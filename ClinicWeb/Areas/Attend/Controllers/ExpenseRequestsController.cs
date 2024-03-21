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
    public class ExpenseRequestsController : Controller
    {
        private readonly ClinicSysContext _context;

        public ExpenseRequestsController(ClinicSysContext context)
        {
            _context = context;
        }

        // GET: Attend/ExpenseRequests
        public async Task<IActionResult> ExpenseRequests()
        {
            var clinicSysContext = _context.AttendanceTExpenseRequests.Include(a => a.FEmployee).Include(a => a.FExpenseType);
            return View(await clinicSysContext.ToListAsync());
        }

        // GET: Attend/ExpenseRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AttendanceTExpenseRequests == null)
            {
                return NotFound();
            }

            var attendanceTExpenseRequests = await _context.AttendanceTExpenseRequests
                .Include(a => a.FEmployee)
                .Include(a => a.FExpenseType)
                .FirstOrDefaultAsync(m => m.FRequestId == id);
            if (attendanceTExpenseRequests == null)
            {
                return NotFound();
            }

            return View(attendanceTExpenseRequests);
        }

        // GET: Attend/ExpenseRequests/Create
        public IActionResult Create()
        {
            ViewData["FEmployeeId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address");
            ViewData["FExpenseTypeId"] = new SelectList(_context.AttendanceTExpenseTypes, "FExpenseTypeId", "FExpenseTypeId");
            return View();
        }

        // POST: Attend/ExpenseRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FRequestId,FEmployeeId,FExpenseTypeId,FAmount,FRequestDate,FExpenseDate,FRequestsDescription,FApprovalStatus,FPayStatus")] AttendanceTExpenseRequests attendanceTExpenseRequests)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendanceTExpenseRequests);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FEmployeeId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", attendanceTExpenseRequests.FEmployeeId);
            ViewData["FExpenseTypeId"] = new SelectList(_context.AttendanceTExpenseTypes, "FExpenseTypeId", "FExpenseTypeId", attendanceTExpenseRequests.FExpenseTypeId);
            return View(attendanceTExpenseRequests);
        }

        // GET: Attend/ExpenseRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AttendanceTExpenseRequests == null)
            {
                return NotFound();
            }

            var attendanceTExpenseRequests = await _context.AttendanceTExpenseRequests.FindAsync(id);
            if (attendanceTExpenseRequests == null)
            {
                return NotFound();
            }
            ViewData["FEmployeeId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", attendanceTExpenseRequests.FEmployeeId);
            ViewData["FExpenseTypeId"] = new SelectList(_context.AttendanceTExpenseTypes, "FExpenseTypeId", "FExpenseTypeId", attendanceTExpenseRequests.FExpenseTypeId);
            return View(attendanceTExpenseRequests);
        }

        // POST: Attend/ExpenseRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FRequestId,FEmployeeId,FExpenseTypeId,FAmount,FRequestDate,FExpenseDate,FRequestsDescription,FApprovalStatus,FPayStatus")] AttendanceTExpenseRequests attendanceTExpenseRequests)
        {
            if (id != attendanceTExpenseRequests.FRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendanceTExpenseRequests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceTExpenseRequestsExists(attendanceTExpenseRequests.FRequestId))
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
            ViewData["FEmployeeId"] = new SelectList(_context.MemberEmployeeList, "EmpId", "Address", attendanceTExpenseRequests.FEmployeeId);
            ViewData["FExpenseTypeId"] = new SelectList(_context.AttendanceTExpenseTypes, "FExpenseTypeId", "FExpenseTypeId", attendanceTExpenseRequests.FExpenseTypeId);
            return View(attendanceTExpenseRequests);
        }

        // GET: Attend/ExpenseRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AttendanceTExpenseRequests == null)
            {
                return NotFound();
            }

            var attendanceTExpenseRequests = await _context.AttendanceTExpenseRequests
                .Include(a => a.FEmployee)
                .Include(a => a.FExpenseType)
                .FirstOrDefaultAsync(m => m.FRequestId == id);
            if (attendanceTExpenseRequests == null)
            {
                return NotFound();
            }

            return View(attendanceTExpenseRequests);
        }

        // POST: Attend/ExpenseRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AttendanceTExpenseRequests == null)
            {
                return Problem("Entity set 'ClinicSysContext.AttendanceTExpenseRequests'  is null.");
            }
            var attendanceTExpenseRequests = await _context.AttendanceTExpenseRequests.FindAsync(id);
            if (attendanceTExpenseRequests != null)
            {
                _context.AttendanceTExpenseRequests.Remove(attendanceTExpenseRequests);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceTExpenseRequestsExists(int id)
        {
          return (_context.AttendanceTExpenseRequests?.Any(e => e.FRequestId == id)).GetValueOrDefault();
        }
    }
}
