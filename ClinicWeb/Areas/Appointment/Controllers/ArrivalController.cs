using ClinicWeb.Areas.Appointment.Models;
using ClinicWeb.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace ClinicWeb.Areas.Appointment.Controllers
{
	[Area("Appointment")]
	public class ArrivalController : Controller
	{
		private ClinicSysContext _context;
		private IHubContext<ApptStateHub, IApptStateHub> _hub { get; set; }
		public ArrivalController(ClinicSysContext context, IHubContext<ApptStateHub, IApptStateHub> hub)
		{
			_context = context;
			_hub = hub;
		}

		public IActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// 撈取 報到管理 當日報到病患資訊
		/// </summary>
		/// <param name="year"></param>
		/// <param name="month"></param>
		/// <param name="day"></param>
		/// <param name="clinicAppt_id">選填，若有則只回傳該筆</param>
		/// <returns></returns>
		[HttpGet]
		[Route("{area}/{controller}/{action}/{year}/{month}/{day}/{clinicAppt_id?}")]
		public IActionResult Get_ArrivalManagerList(string year, string month, string day, string? clinicAppt_id)
		{
			string dateString = $"{year}/{month}/{day}";
			if (string.IsNullOrEmpty(clinicAppt_id))
			{
				return Json(_context.ApptClinicList.Where(x => x.Clinic.Date == dateString)
					.Select(x => new
					{
						clinicAppt_id = x.ClinicListId,
						national_id = x.Member.NationalId,
						姓名 = x.Member.Name,
						日期 = x.Clinic.Date,
						時段 = x.Clinic.ClinicTime.ClinicShifts,
						科別 = x.Clinic.Doctor.Department,
						醫師名稱 = x.Clinic.Doctor.Name,
						診號 = x.ClinicNumber,
						繳費狀態 = x.ApptEcpay.FirstOrDefault().RtnMsg == "已付款" ? "已付款" : "未繳費",
						看診狀態 = x.PatientState.PatientStateName
					})
					);
			}
			else
			{
				return Json(_context.ApptClinicList.Where(x => x.Clinic.Date == dateString && x.ClinicListId == Convert.ToInt32(clinicAppt_id))
					.Select(x => new
					{
						clinicAppt_id = x.ClinicListId,
						national_id = x.Member.NationalId,
						姓名 = x.Member.Name,
						日期 = x.Clinic.Date,
						時段 = x.Clinic.ClinicTime.ClinicShifts,
						科別 = x.Clinic.Doctor.Department,
						醫師名稱 = x.Clinic.Doctor.Name,
						診號 = x.ClinicNumber,
						繳費狀態 = x.ApptEcpay.FirstOrDefault().RtnMsg == "已付款" ? "已付款" : "未繳費",
						看診狀態 = x.PatientState.PatientStateName
					})
					.FirstOrDefault()
					);
			}

		}

        [AllowAnonymous]
        [HttpGet]
        [Route("{area}/{controller}/{action}/{clinicAppt_id}")]
        public async Task<IActionResult> Go_CheckIn(string clinicAppt_id)
        {
            //查詢有無掛號紀錄
            var target = _context.ApptClinicList
                .Where(x => x.ClinicListId == Convert.ToInt32(clinicAppt_id));
            if (target.Count() == 0) { return NotFound(); }

            var patientInfo = target.Select(x => new
            {
                ClinicId = x.ClinicId,
                ClinicListId = x.ClinicListId,
                ClinicNumber = x.ClinicNumber,
                PatientStateId = x.PatientStateId,
            }).FirstOrDefault();
            //Console.WriteLine($"ClinicListId: {patientInfo.ClinicListId}");
            //Console.WriteLine($"ClinicNumber: {patientInfo.ClinicNumber}");

            //是否已報到
            if (patientInfo.PatientStateId != 8)
            {
                return Json(new
                {
                    department = "",
                    shift = "",
                    doctor = "",
                    number = -2,
                    state = patientInfo.PatientStateId
                });
            }

            //查詢目前看診號
            int currentNumber = 0;
            try
            {
                currentNumber = _context.ApptClinicList
                .Where(x => x.ClinicId == patientInfo.ClinicId
                && (x.PatientStateId == 1 || x.PatientStateId == 2 || x.PatientStateId == 4 || x.PatientStateId == 5 || x.PatientStateId == 6 || x.PatientStateId == 7)
                )
                .Select(x => x.ClinicNumber)
                .Max();
            }
            catch (Exception)
            { }

            Console.WriteLine($"currentNum Max: {currentNumber}");

            //是否遲到
            bool isLate = (currentNumber > patientInfo.ClinicNumber);
            //Console.WriteLine($"isLate: {isLate}");

            //更新資料庫
            target.FirstOrDefault().PatientStateId = isLate ? 4 : 3;
            await _context.SaveChangesAsync();

            //websocket連線 更新醫師看診畫面
            var selMessage = target.Select(x => new
            {
                member_id = x.MemberId,
                clinicListId = x.ClinicListId,
                status_id = x.PatientStateId,
                診號 = x.ClinicNumber,
                姓名 = x.Member.Name,
                性別 = (bool)x.Member.Gender ? "男" : "女",
                狀態 = x.PatientState.PatientStateName
            }).FirstOrDefault();
            await _hub.Clients.All.Set_State(selMessage.ToJson());

            //回傳報到狀態 給報到機
            var selResult = target.Select(x => new
            {
                clinicAppt_id = x.ClinicListId,
                日期 = x.Clinic.Date,
                時段 = x.Clinic.ClinicTime.ClinicShifts,
                科別 = x.Clinic.Doctor.Department,
                醫師名稱 = x.Clinic.Doctor.Name,
                診號 = x.ClinicNumber,
                報到狀態 = x.PatientState.PatientStateName
            }).FirstOrDefault();

            //新增付款資訊到資料庫
            int clinicListId = target.FirstOrDefault().ClinicListId;
            var target2 = _context.ApptEcpay.Where(x => x.FClinicListId == clinicListId);
            if (target2.Count() == 0)
            {
                ApptEcpay pay = new ApptEcpay()
                {
                    FClinicListId = target.FirstOrDefault().ClinicListId,
                    FisNhi = true,
                    FPayable = 150
                };
                _context.ApptEcpay.Add(pay);
                await _context.SaveChangesAsync();
            }

            return Json(selResult);
        }

        /// <summary>
        /// [棄用] 舊方法!!!
        /// 執行報到
        /// 健保卡插卡 與 診所後臺管理 共用通道
        /// </summary>
        /// <param name="nationalId">身分證字號</param>
        /// <returns></returns>
        [AllowAnonymous]
		[HttpGet]
		[Route("{area}/{controller}/{action}/{year}/{month}/{day}/{nationalId}")]
		public async Task<IActionResult> GET_ArrivalAsync(string year, string month, string day, string nationalId)
		{
			//查詢有無掛號紀錄
			string dateString = $"{year}/{month}/{day}";
			var target = _context.ApptClinicList
				.Where(x => x.Member.NationalId == nationalId && x.Clinic.Date == dateString);
			if (target.Count() == 0) { return NotFound(); }

			var patientInfo = target.Select(x => new
			{
				ClinicId = x.ClinicId,
				ClinicListId = x.ClinicListId,
				ClinicNumber = x.ClinicNumber,
				PatientStateId = x.PatientStateId,
			}).FirstOrDefault();
			//Console.WriteLine($"ClinicListId: {patientInfo.ClinicListId}");
			//Console.WriteLine($"ClinicNumber: {patientInfo.ClinicNumber}");

			//是否已報到
			if (patientInfo.PatientStateId != 8)
			{
				return Json(new
				{
					department = "",
					shift = "",
					doctor = "",
					number = -2,
					state = patientInfo.PatientStateId
				});
			}

			//查詢目前看診號
			int currentNumber = 0;
			try
			{
				currentNumber = _context.ApptClinicList
				.Where(x => x.ClinicId == patientInfo.ClinicId
				&& (x.PatientStateId == 1 || x.PatientStateId == 2 || x.PatientStateId == 4 || x.PatientStateId == 5 || x.PatientStateId == 6 || x.PatientStateId == 7)
				)
				.Select(x => x.ClinicNumber)
				.Max();
			}
			catch (Exception)
			{ }

			Console.WriteLine($"currentNum Max: {currentNumber}");

			//是否遲到
			bool isLate = (currentNumber > patientInfo.ClinicNumber);
			//Console.WriteLine($"isLate: {isLate}");

			//更新資料庫
			target.FirstOrDefault().PatientStateId = isLate ? 4 : 3;
			await _context.SaveChangesAsync();

			//websocket連線 更新醫師看診畫面
			var selMessage = target.Select(x => new
			{
				member_id = x.MemberId,
				clinicListId = x.ClinicListId,
				status_id = x.PatientStateId,
				診號 = x.ClinicNumber,
				姓名 = x.Member.Name,
				性別 = (bool)x.Member.Gender ? "男" : "女",
				狀態 = x.PatientState.PatientStateName
			}).FirstOrDefault();
			await _hub.Clients.All.Set_State(selMessage.ToJson());

			//回傳報到狀態 給報到機
			var selResult = target.Select(x => new
			{
				department = x.Clinic.Doctor.Department,
				shift = x.Clinic.ClinicTime.ClinicShifts,
				doctor = x.Clinic.Doctor.Name,
				number = x.ClinicNumber,
				state = isLate ? 4 : 3
			}).FirstOrDefault();

			//新增付款資訊到資料庫
			int clinicListId = target.FirstOrDefault().ClinicListId;
			var target2 = _context.ApptEcpay.Where(x => x.FClinicListId == clinicListId);
			if (target2.Count() == 0)
			{
				ApptEcpay pay = new ApptEcpay()
				{
					FClinicListId = target.FirstOrDefault().ClinicListId,
					FisNhi = true,
					FPayable = 150
				};
				_context.ApptEcpay.Add(pay);
				await _context.SaveChangesAsync();
			}

			return Json(selResult);
		}
	}
}
