using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicWeb.Models;

namespace ClinicWeb.Areas.Attend.Controllers
{
    [Area("Attend")]
    public class LeavesController : Controller
    {
        private readonly ClinicSysContext _context;

        public LeavesController(ClinicSysContext context)
        {
            _context = context;
        }

        // GET: Attend/Leaves
        public async Task<IActionResult> Index()
        {
            var clinicSysContext = _context.AttendanceTLeave.Include(a => a.FEmployee).Include(a => a.FLeaveType);
            return View(await clinicSysContext.ToListAsync());
        }

        // GET: Attend/Leaves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AttendanceTLeave == null)
            {
                return NotFound();
            }

            var attendanceTLeave = await _context.AttendanceTLeave
                .Include(a => a.FEmployee)
                .Include(a => a.FLeaveType)
                .FirstOrDefaultAsync(m => m.FLeaveId == id);
            if (attendanceTLeave == null)
            {
                return NotFound();
            }

            return View(attendanceTLeave);
        }

        // GET: Attend/Leaves/Create
        public IActionResult Create()
        {
            ViewData["FEmployeeId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address");
            ViewData["FLeaveTypeId"] = new SelectList(_context.AttendanceTLeaveTypes, "FLeaveTypeId", "FLeaveTypeId");
            return View();
        }

        // POST: Attend/Leaves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FLeaveId,FEmployeeId,FLeaveTypeId,FStartDate,FEndDate,FSubstitute,FLeaveStatus,FLeaveDescription")] AttendanceTLeave attendanceTLeave)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendanceTLeave);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FEmployeeId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", attendanceTLeave.FEmployeeId);
            ViewData["FLeaveTypeId"] = new SelectList(_context.AttendanceTLeaveTypes, "FLeaveTypeId", "FLeaveTypeId", attendanceTLeave.FLeaveTypeId);
            return View(attendanceTLeave);
        }

        // GET: Attend/Leaves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AttendanceTLeave == null)
            {
                return NotFound();
            }

            var attendanceTLeave = await _context.AttendanceTLeave.FindAsync(id);
            if (attendanceTLeave == null)
            {
                return NotFound();
            }
            ViewData["FEmployeeId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", attendanceTLeave.FEmployeeId);
            ViewData["FLeaveTypeId"] = new SelectList(_context.AttendanceTLeaveTypes, "FLeaveTypeId", "FLeaveTypeId", attendanceTLeave.FLeaveTypeId);
            return View(attendanceTLeave);
        }

        // POST: Attend/Leaves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FLeaveId,FEmployeeId,FLeaveTypeId,FStartDate,FEndDate,FSubstitute,FLeaveStatus,FLeaveDescription")] AttendanceTLeave attendanceTLeave)
        {
            if (id != attendanceTLeave.FLeaveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendanceTLeave);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceTLeaveExists(attendanceTLeave.FLeaveId))
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
            ViewData["FEmployeeId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", attendanceTLeave.FEmployeeId);
            ViewData["FLeaveTypeId"] = new SelectList(_context.AttendanceTLeaveTypes, "FLeaveTypeId", "FLeaveTypeId", attendanceTLeave.FLeaveTypeId);
            return View(attendanceTLeave);
        }

        // GET: Attend/Leaves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AttendanceTLeave == null)
            {
                return NotFound();
            }

            var attendanceTLeave = await _context.AttendanceTLeave
                .Include(a => a.FEmployee)
                .Include(a => a.FLeaveType)
                .FirstOrDefaultAsync(m => m.FLeaveId == id);
            if (attendanceTLeave == null)
            {
                return NotFound();
            }

            return View(attendanceTLeave);
        }

        // POST: Attend/Leaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AttendanceTLeave == null)
            {
                return Problem("Entity set 'ClinicSysContext.AttendanceTLeave'  is null.");
            }
            var attendanceTLeave = await _context.AttendanceTLeave.FindAsync(id);
            if (attendanceTLeave != null)
            {
                _context.AttendanceTLeave.Remove(attendanceTLeave);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceTLeaveExists(int id)
        {
          return (_context.AttendanceTLeave?.Any(e => e.FLeaveId == id)).GetValueOrDefault();
        }
    }
}
