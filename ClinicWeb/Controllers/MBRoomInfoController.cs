using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ClinicWeb.Areas.Room.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ClinicWeb.Controllers.MBRoomInfoController
{
    [Authorize(Policy = "frontendpolicy")]
    public class MBRoomInfoController : Controller
    {
        private readonly ClinicSysContext _context;

        public MBRoomInfoController(ClinicSysContext context)
        {
            _context = context;
        }

        [HttpGet]
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
                
                string userName = User.Identity.Name; 

                if (order.StartDate >= order.EndDate)
                {
                    TempData["ErrorMessage"] = "結束日期必須大於開始日期";
                    return RedirectToAction(nameof(Index));
                }

                var existingAppointments = _context.AppointmentRoomSchedule
                    .Where(a => a.RoomId == order.RoomId &&
                                ((a.StartDate < order.EndDate && a.EndDate > order.StartDate) ||
                                (a.StartDate <= order.StartDate && a.EndDate >= order.EndDate)))
                    .ToList();

                if (existingAppointments.Any())
                {
                    TempData["ErrorMessage"] = "該時間段已有預約";
                    return RedirectToAction(nameof(Index));
                }

                int memid = _context.MemberMemberList.Where(x => x.Name == userName).Select(x=>x.MemberId).FirstOrDefault();
                //order.Member.Name = userName;
                order.MemberId = memid;

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

