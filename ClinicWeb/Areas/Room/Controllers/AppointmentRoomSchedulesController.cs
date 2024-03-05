using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicWeb.Areas.Room.Models;

namespace ClinicWeb.Areas.Room.Controllers
{
    [Area("Room")]
    public class AppointmentRoomSchedulesController : Controller
    {
        private readonly ClinicSysContext _context;

        public AppointmentRoomSchedulesController(ClinicSysContext context)
        {
            _context = context;
        }

        // GET: Room/AppointmentRoomSchedules
        public async Task<IActionResult> Index()
        {
            var clinicSysContext = _context.AppointmentRoomSchedule.Include(a => a.Doctor).Include(a => a.Member).Include(a => a.Nurse).Include(a => a.Room);
            return View(await clinicSysContext.ToListAsync());
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
            ViewData["DoctorId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address");
            ViewData["MemberId"] = new SelectList(_context.MemberMemberList, "MemberId", "Address");
            ViewData["NurseId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address");
            ViewData["RoomId"] = new SelectList(_context.RoomList, "RoomId", "RoomId");
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

        // POST: Room/AppointmentRoomSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AppointmentRoomSchedule == null)
            {
                return Problem("Entity set 'ClinicSysContext.AppointmentRoomSchedule'  is null.");
            }
            var appointmentRoomSchedule = await _context.AppointmentRoomSchedule.FindAsync(id);
            if (appointmentRoomSchedule != null)
            {
                _context.AppointmentRoomSchedule.Remove(appointmentRoomSchedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentRoomScheduleExists(int id)
        {
          return (_context.AppointmentRoomSchedule?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
        }
    }
}
