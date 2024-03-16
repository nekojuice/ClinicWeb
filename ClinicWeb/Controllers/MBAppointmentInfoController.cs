using ClinicWeb.Areas.Appointment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ClinicWeb.Controllers
{
    [Authorize(Policy = "frontendpolicy")]
    public class MBAppointmentInfoController : Controller
    {
        private readonly ClinicSysContext _context;

        public MBAppointmentInfoController(ClinicSysContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
			var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
			var website = $"https://localhost:7071"; //記得確認一下數字有沒有一樣
			var order = new Dictionary<string, string>
				{
					{ "MerchantTradeNo",  orderId},
					{ "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
					{ "TotalAmount",  "150"},
					{ "TradeDesc",  "無"},
					{ "ItemName",  "寶育診所掛號費"},
					{ "ExpireDate",  "3"},
					{ "CustomField1",  ""},
					{ "CustomField2",  ""},
					{ "CustomField3",  ""},
					{ "CustomField4",  ""},
					{ "ReturnURL",  $"{website}/ECPay/AddPayInfo"}, //付款完成通知回傳的網址(無用)
                    { "OrderResultURL", $"{website}/ECPay/PayInfo/{orderId}"},//瀏覽器端回傳付款結果網址
                    { "PaymentInfoURL",  $"{website}/ECPay/AddAccountInfo"},//伺服器端回傳付款相關資訊(無用)
                    { "ClientRedirectURL",  $"{website}/ECPay/AccountInfo/{orderId}"},//Client 端回傳付款相關資訊
                    { "MerchantID",  "2000132"},
					{ "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
					{ "PaymentType",  "aio"},
					{ "ChoosePayment",  "ALL"},
					{ "EncryptType",  "1"},
				};
			order["CheckMacValue"] = GetCheckMacValue(order);
			return View(order);
		}
		private string GetCheckMacValue(Dictionary<string, string> order)
		{
			var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
			var checkValue = string.Join("&", param);
			//測試用的 HashKey
			var hashKey = "5294y06JbISpM5x9";
			//測試用的 HashIV
			var HashIV = "v77hoKGq4kWxNNIS";
			checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
			checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
			checkValue = GetSHA256(checkValue);
			return checkValue.ToUpper();
		}
		private string GetSHA256(string value)
		{
			var result = new StringBuilder();
			var sha256 = SHA256.Create();
			var bts = Encoding.UTF8.GetBytes(value);
			var hash = sha256.ComputeHash(bts);
			for (int i = 0; i < hash.Length; i++)
			{
				result.Append(hash[i].ToString("X2"));
			}
			return result.ToString();
		}

		/// <summary>
		/// 撈取使用者登入資訊
		/// </summary>
		/// <returns>jsonobject</returns>
		public IActionResult Get_MemberStatus()
        {
            var user = HttpContext.User;
            var resultObject = new
            {
                MemberNumber = user.Claims.FirstOrDefault(c => c.Type == "MemberNumber")?.Value,
                MemberName = user.Claims.FirstOrDefault(c => c.Type == "MemberName")?.Value,
                MemberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value
            };
            return Json(resultObject);
        }

        /// <summary>
        /// 撈取該使用者 當前日期以後 的掛號資訊
        /// </summary>
        /// <param name="vm">json物件 成員: today (DateTime)</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Get_MemberAppt([FromBody] DateViewModel vm)
        {
            Console.WriteLine(vm.today.ToString("yyyy/MM/dd"));
            var user = HttpContext.User;
            var memberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value;

            return Json(_context.ApptClinicList
                .Where(x => x.MemberId == Convert.ToInt32(memberID)
                && x.Clinic.Date.CompareTo(vm.today.ToString("yyyy/MM/dd")) >= 0
                && x.IsCancelled == false)
                .Select(x => new
                {
                    id = x.ClinicListId,
                    日期 = x.Clinic.Date,
                    時段 = x.Clinic.ClinicTime.ClinicShifts,
                    科別 = x.Clinic.Doctor.Department,
                    醫師名稱 = x.Clinic.Doctor.Name,
                    診間 = x.Clinic.ClincRoom.Name,
                    診號 = x.ClinicNumber,
                    看診狀態 = x.PatientState.PatientStateName,
                })
                );
        }

        /// <summary>
        /// 退掛指定掛號清單id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Put_CancelMemberAppt(string id)
        {
            try
            {
                var target = _context.ApptClinicList.Where(x => x.ClinicListId == Convert.ToInt32(id)).FirstOrDefault();
                if (target == null) { return NotFound(); }
                else
                {
                    target.IsCancelled = true;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("[偵錯] db update error!!!!");
                return BadRequest();
            }
            Console.WriteLine("[偵錯] else error!!!!");
            return Ok();
        }


        [HttpGet]
        public IActionResult Get_PaitientPayment() 
        {
			var user = HttpContext.User;
			var memberID = user.Claims.FirstOrDefault(c => c.Type == "MemberID")?.Value;

			return Json(_context.ApptEcpay.Where(x=>x.FClinicList.MemberId == Convert.ToInt32(memberID))
                .Select(x => new {
					apptlistid = x.FClinicListId,
					日期 = x.FClinicList.Clinic.Date,
					時段 = x.FClinicList.Clinic.ClinicTime.ClinicShifts,
					科別 = x.FClinicList.Clinic.Doctor.Department,
					醫師名稱 = x.FClinicList.Clinic.Doctor.Name,
					看診狀態 = x.FClinicList.PatientState.PatientStateName,
					繳費狀態 = x.RtnMsg == "已付款"? "已付款":"未付款"
				})
                );
        }
    }

    //接收日期用viewmodel
    public class DateViewModel
    {
        public DateTime today { get; set; }
    }
}
