using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicWeb.Areas.Member.Models;

namespace ClinicWeb.Areas.Member.Controllers
{
    [Area("Member")]
    public class EmpController : Controller
    {
        private readonly ClinicSysContext _context;

        public EmpController(ClinicSysContext context)
        {
            _context = context;
        }

        // GET: Member/Emp
        public async Task<IActionResult> Index()
        {
            return View() ;
                        
        }

        // GET: Member/Emp/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MemberEmployeeList == null)
            {
                return NotFound();
            }

            var memberEmployeeList = await _context.MemberEmployeeList
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (memberEmployeeList == null)
            {
                return NotFound();
            }

            return View(memberEmployeeList);
        }

        // GET: Member/Emp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Member/Emp/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpId,StaffNumber,Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,EmpType,Department,EmpPassword,EmpPhoto,Quit")] MemberEmployeeList memberEmployeeList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memberEmployeeList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(memberEmployeeList);
        }

        // GET: Member/Emp/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MemberEmployeeList == null)
            {
                return NotFound();
            }

            var memberEmployeeList = await _context.MemberEmployeeList.FindAsync(id);
            if (memberEmployeeList == null)
            {
                return NotFound();
            }
            return View(memberEmployeeList);
        }

        // POST: Member/Emp/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpId,StaffNumber,Name,Gender,BloodType,NationalId,Address,ContactAddress,Phone,BirthDate,EmpType,Department,EmpPassword,EmpPhoto,Quit")] MemberEmployeeList memberEmployeeList)
        {
            if (id != memberEmployeeList.EmpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberEmployeeList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberEmployeeListExists(memberEmployeeList.EmpId))
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
            return View(memberEmployeeList);
        }

        // GET: Member/Emp/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MemberEmployeeList == null)
            {
                return NotFound();
            }

            var memberEmployeeList = await _context.MemberEmployeeList
                .FirstOrDefaultAsync(m => m.EmpId == id);
            if (memberEmployeeList == null)
            {
                return NotFound();
            }

            return View(memberEmployeeList);
        }

        // POST: Member/Emp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MemberEmployeeList == null)
            {
                return Problem("Entity set 'ClinicSysContext.MemberEmployeeList'  is null.");
            }
            var memberEmployeeList = await _context.MemberEmployeeList.FindAsync(id);
            if (memberEmployeeList != null)
            {
                _context.MemberEmployeeList.Remove(memberEmployeeList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberEmployeeListExists(int id)
        {
          return (_context.MemberEmployeeList?.Any(e => e.EmpId == id)).GetValueOrDefault();
        }
    }
}
