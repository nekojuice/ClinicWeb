using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ClinicWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicWeb.Areas.Room.Controllers
{
    public class MBRoomInfoController : Controller
    {
        private readonly ClinicSysContext _context;

        public MBRoomInfoController(ClinicSysContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rooms = _context.RoomList.Where(r => r.TypeId == 4).ToList();
            ViewData["Rooms"] = new SelectList(rooms, "RoomId", "Name");
            ViewData["Doctors"] = _context.MemberEmployeeList.Where(e => e.EmpType == "醫生").ToList();
            ViewData["Nurses"] = _context.MemberEmployeeList.Where(e => e.EmpType == "護士").ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(AppointmentRoomSchedule order)
        {
            if (ModelState.IsValid)
            {
                DateTime startDate, endDate;
                if (!DateTime.TryParseExact(order.StartDate, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out startDate) ||
                    !DateTime.TryParseExact(order.EndDate, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out endDate))
                {
                    TempData["ErrorMessage"] = "日期格式無效，請使用 yyyy/MM/dd 格式。";
                    return RedirectToAction(nameof(Index));
                }

                var query = _context.AppointmentRoomSchedule
                    .Where(a => a.RoomId == 4 && (DateTime.Parse(a.StartDate) >= startDate || DateTime.Parse(a.EndDate) <= endDate))
                    .ToList();

                //新增到資料庫
                _context.AppointmentRoomSchedule.Add(order);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "預約成功！";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "預約失敗，請填寫正確的訊息。";
            }

            var rooms = _context.RoomList.Where(r => r.TypeId == 4).ToList();
            ViewData["Rooms"] = rooms.Any() ? new SelectList(rooms, "RoomId", "Name") : new SelectList(new List<RoomList>());
            ViewData["Doctors"] = _context.MemberEmployeeList.Where(e => e.EmpType == "醫生").ToList();
            ViewData["Nurses"] = _context.MemberEmployeeList.Where(e => e.EmpType == "護士").ToList();
            return View("Index", order);
        }
    }
}
