using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClinicWeb.Areas.Attend.Controllers
{


	[Area("Attend")]
	public class AttController : Controller
	{
		private readonly ClinicSysContext _context;
		public AttController(ClinicSysContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var employees = _context.AttendanceTAttendance.Select(x => x.FEmployeeId).ToList().Distinct();
			ViewBag.employees = employees;
			return View();
		}

		public IActionResult Check()
		{
			return View();
		}
        public IActionResult Leave()
		{
			 var substitute =_context.AttendanceTLeave.Select(x => x.FSubstitute).Distinct();
			var leaveName = _context.AttendanceTLeaveTypes.Select(x => x.FLeaveTypeName).Distinct();
			var employees = _context.AttendanceTLeave.Select(x => x.FEmployeeId).ToList().Distinct();
			ViewBag.leaveName = leaveName;
			ViewBag.employees = employees;
			ViewBag.substitute=substitute;
			return View();
		}
		public IActionResult ExpenseRequests()
		{
			var employees = _context.AttendanceTExpenseRequests.Select(x => x.FEmployeeId).ToList().Distinct();
			ViewBag.employees = employees;
			return View();
		}
		[Route("{area}/{controller}/{action}/{id}")]
		public JsonResult checkData(string id)
		{
			return Json(_context.AttendanceTAttendance
				.Where(x => x.FEmployeeId == Convert.ToInt32(id))
			.Select(x => new
			{
					上班時間 = x.FCheckInTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
					下班時間 = x.FCheckOutTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
					上班日期 = x.FWorkDate.Value.ToString("yyyy-MM-dd"),
					上班狀態 = x.FAttendanceCis,
					下班狀態 = x.FAttendanceCos
				}));
		}
		public JsonResult LeaveData(string id)
		{
			return Json(_context.AttendanceTLeave
				.Where(x => x.FEmployeeId == Convert.ToInt32(id))
				.Select(x => new
				{
					員工名稱 = x.FEmployee.Name,
					假別 = x.FLeaveType.FLeaveTypeName,
					請假起日 = x.FStartDate.Value.ToString("yyyy-MM-dd"),
					請假迄日 = x.FEndDate.Value.ToString("yyyy-MM-dd"),
					代理人 = x.FSubstitute,
					申請狀態 =x.FLeaveStatus
				}));
		}


		public JsonResult ExpenseData(string id)
		{
			return Json(_context.AttendanceTExpenseRequests
				.Where(x => x.FEmployeeId == Convert.ToInt32(id))
				.Select(x => new
				{
					員工名稱 = x.FEmployee.Name,
					類別 = x.FExpenseType.FExpenseTypeName,
					費用申請日 = x.FRequestDate.Value.ToString("yyyy-MM-dd"),
					費用核發日 = x.FExpenseDate.Value.ToString("yyyy-MM-dd"),
					申請金額 = x.FAmount,
					申請狀態 = x.FApprovalStatus
				}));
		}
		[Route("{area}/{controller}/{action}")]
		[HttpPost]
		public IActionResult Create([FromBody] AttendanceTLeave data)
		{
			if (data == null)
			{
				return BadRequest("Invalid data");
			}

			var lanpa = new AttendanceTLeave
			{
				FEmployeeId = data.FEmployeeId,
				FLeaveDescription = data.FLeaveDescription,
				FStartDate = data.FStartDate,
				FEndDate = data.FEndDate,
				FSubstitute = data.FSubstitute,
				FLeaveTypeId = data.FLeaveTypeId,
				FLeaveStatus = "待審核"
		};

			try
			{
				_context.AttendanceTLeave.Add(lanpa);
				_context.SaveChanges();
				return Ok("申請成功");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}


		[Route("{area}/{controller}/{action}")]
		[HttpPost]
		public IActionResult CheckIn([FromBody] AttendanceTAttendance data)
		{

			if (data == null)
			{
				return BadRequest("Invalid data");
			}


			var attendanceRecord = _context.AttendanceTAttendance
				.FirstOrDefault(x => x.FEmployeeId == data.FEmployeeId && x.FWorkDate == data.FWorkDate);
			if (attendanceRecord != null)
			{
				return BadRequest("已打上班卡");
			}


			try
			{
				_context.AttendanceTAttendance.Add(data);
				_context.SaveChanges();
				return Ok("上班打卡成功");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}



		[Route("{area}/{controller}/{action}")]
		[HttpPost]
		public IActionResult CheckOut([FromBody] AttendanceTAttendance data)
		{
			if (data == null)
			{
				return BadRequest("Invalid data");
			}
			var attendanceRecord = _context.AttendanceTAttendance
				.FirstOrDefault(x => x.FEmployeeId == data.FEmployeeId && x.FWorkDate == data.FWorkDate);

			if (attendanceRecord == null)
			{
				return BadRequest("未打上班卡");
			}

			if (attendanceRecord.FAttendanceCos != null)
			{
				return BadRequest("已打下班卡");
			}

			try
			{
				attendanceRecord.FCheckOutTime = data.FCheckOutTime;
				attendanceRecord.FAttendanceCos = data.FAttendanceCos;


				_context.AttendanceTAttendance.Update(attendanceRecord);
				_context.SaveChanges();
				return Ok("下班打卡成功");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}


		}

		[Route("{area}/{controller}/{action}")]
		public JsonResult DateList(string id)
		{
			return Json(_context.AttendanceTAttendance
				.Where(x => x.FEmployeeId == Convert.ToInt32(id))
				.Select(x => x.FWorkDate.HasValue ? x.FWorkDate.Value.ToString("yyyy-MM-dd") : null)
				.Distinct());
		}
	}
}