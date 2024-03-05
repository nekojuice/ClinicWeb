using Microsoft.AspNetCore.Mvc;
using ClinicWeb.Areas.Room.Models;

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicWeb.Areas.Room.Controllers
{
    [Area("Room")]
    public class RoomController : Controller
    {
        private readonly ClinicSysContext _context;

        public RoomController(ClinicSysContext context)
        {
            _context = context;
        }





        public IActionResult Index()
        {
            return View(_context.AppointmentRoomSchedule
                .Select(x => new ShowAppointmentRoomSchedule()
                {
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DoctorName = x.Doctor.Name,
                    NurseName = x.Nurse.Name,
                    MemberName = x.Member.Name,
                    RoomName = x.Room.Name,
                }));
        }

        // GET: Room/AppointmentRoomSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AppointmentRoomSchedule == null)
            {
                return NotFound();
            }

            var appointmentRoomSchedule = await _context.AppointmentRoomSchedule
                .Include(a => a.Doctor)
                .Include(a => a.Member)
                .Include(a => a.Nurse)
                .Include(a => a.Room)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointmentRoomSchedule == null)
            {
                return NotFound();
            }

            return View(appointmentRoomSchedule);
        }

        // GET: Room/AppointmentRoomSchedules/Create

        public IActionResult Create()
        {
            ViewBag.RoomId = new SelectList(_context.RoomList, "RoomId", "Name");
            ViewBag.MemberId = new SelectList(_context.MemberMemberList, "MemberId", "Name");
            ViewBag.DoctorId = new SelectList(_context.MemberEmployeeList.Where(x => x.EmpType == "醫生"), "EmpId", "Name");
            ViewBag.NurseId = new SelectList(_context.MemberEmployeeList.Where(x => x.EmpType == "護士"), "EmpId", "Name");
            return View();
        }




        // POST: Room/AppointmentRoomSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,RoomId,MemberId,StartDate,EndDate,DoctorId,NurseId")] AppointmentRoomSchedule appointmentRoomSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentRoomSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", appointmentRoomSchedule.DoctorId);
            ViewData["MemberId"] = new SelectList(_context.MemberMemberList, "MemberId", "Address", appointmentRoomSchedule.MemberId);
            ViewData["NurseId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", appointmentRoomSchedule.NurseId);
            ViewData["RoomId"] = new SelectList(_context.RoomList, "RoomId", "RoomId", appointmentRoomSchedule.RoomId);
            return View(appointmentRoomSchedule);
        }

        // GET: Room/AppointmentRoomSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AppointmentRoomSchedule == null)
            {
                return NotFound();
            }

            var appointmentRoomSchedule = await _context.AppointmentRoomSchedule.FindAsync(id);
            if (appointmentRoomSchedule == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", appointmentRoomSchedule.DoctorId);
            ViewData["MemberId"] = new SelectList(_context.MemberMemberList, "MemberId", "Address", appointmentRoomSchedule.MemberId);
            ViewData["NurseId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", appointmentRoomSchedule.NurseId);
            ViewData["RoomId"] = new SelectList(_context.RoomList, "RoomId", "RoomId", appointmentRoomSchedule.RoomId);
            return View(appointmentRoomSchedule);
        }

        // POST: Room/AppointmentRoomSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,RoomId,MemberId,StartDate,EndDate,DoctorId,NurseId")] AppointmentRoomSchedule appointmentRoomSchedule)
        {
            if (id != appointmentRoomSchedule.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentRoomSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentRoomScheduleExists(appointmentRoomSchedule.AppointmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", appointmentRoomSchedule.DoctorId);
            ViewData["MemberId"] = new SelectList(_context.MemberMemberList, "MemberId", "Address", appointmentRoomSchedule.MemberId);
            ViewData["NurseId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", appointmentRoomSchedule.NurseId);
            ViewData["RoomId"] = new SelectList(_context.RoomList, "RoomId", "RoomId", appointmentRoomSchedule.RoomId);
            return View(appointmentRoomSchedule);
        }

        // GET: Room/AppointmentRoomSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AppointmentRoomSchedule == null)
            {
                return NotFound();
            }

            var appointmentRoomSchedule = await _context.AppointmentRoomSchedule
                .Include(a => a.Doctor)
                .Include(a => a.Member)
                .Include(a => a.Nurse)
                .Include(a => a.Room)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointmentRoomSchedule == null)
            {
                return NotFound();
            }

            return View(appointmentRoomSchedule);
        }

       [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var appointmentRoomSchedule = await _context.AppointmentRoomSchedule.FindAsync(id);
    if (appointmentRoomSchedule == null)
    {
        return NotFound();
    }

    _context.AppointmentRoomSchedule.Remove(appointmentRoomSchedule);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

private bool AppointmentRoomScheduleExists(int id)
{
    return _context.AppointmentRoomSchedule.Any(e => e.AppointmentId == id);
}

    }

}












