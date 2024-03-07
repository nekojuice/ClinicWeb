using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ClinicWeb.Areas.Room.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicWeb.Areas.Room.Controllers
{
    [Area("Room")]
    public class OrderController : Controller
    {
        private readonly ClinicSysContext _context;

        public OrderController(ClinicSysContext context)
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
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppointmentRoomSchedule order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "預約成功！";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "預約失敗，请填寫正確的訊息。";
            }
            var rooms = _context.RoomList.Where(r => r.TypeId == 4).ToList();
            if (rooms.Any())
            {
                ViewData["Rooms"] = new SelectList(rooms, "RoomId", "Name");
            }
            else
            {

                ViewData["Rooms"] = new SelectList(new List<RoomList>());
            }


            ViewData["Rooms"] = new SelectList(_context.RoomList.Where(r => r.TypeId == 4).ToList(), "RoomId", "Name");
            ViewData["Doctors"] = _context.MemberEmployeeList.Where(e => e.EmpType == "醫生").ToList();
            ViewData["Nurses"] = _context.MemberEmployeeList.Where(e => e.EmpType == "護士").ToList();
            return View("Index", order);

        }
    }
}
