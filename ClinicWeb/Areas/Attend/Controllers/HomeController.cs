using ClinicWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Attend.Controllers
{


	[Area("Attend")]
	public class HomeController : Controller
	{
		private readonly ClinicSysContext _context;
		public HomeController(ClinicSysContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var employees = _context.MemberEmployeeList.Select(x => x.EmpId).ToList();
			ViewBag.employees = employees;
			return View();
		}


		[Route("{area}/{controller}/{action}/{id}")]
		public JsonResult check(string id)
		{
			return Json(_context.AttendanceTAttendance
				.Where(x => x.FEmployeeId == Convert.ToInt32(id))
				.Select(x => new
				{
					上班時間 = x.FCheckInTime,
					下班時間 = x.FCheckOutTime,
					上班日期 = x.FWorkDate,
					上班狀態 = x.FAttendanceCis,
					下班狀態 = x.FAttendanceCos
				}));
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