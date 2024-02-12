using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicWeb.Models;

namespace ClinicWeb.Areas.Drugs.Controllers
{
    [Area("Drugs")]
    public class PharmacyTTypeLists1Controller : Controller
    {
        private readonly ClinicSysContext _context;

        public PharmacyTTypeLists1Controller(ClinicSysContext context)
        {
            _context = context;
        }

        // GET: Drugs/PharmacyTTypeLists1
        public async Task<IActionResult> Index()
        {
              return _context.PharmacyTTypeList != null ? 
                          View(await _context.PharmacyTTypeList.ToListAsync()) :
                          Problem("Entity set 'ClinicSysContext.PharmacyTTypeList'  is null.");
        }

        // GET: Drugs/PharmacyTTypeLists1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PharmacyTTypeList == null)
            {
                return NotFound();
            }

            var pharmacyTTypeList = await _context.PharmacyTTypeList
                .FirstOrDefaultAsync(m => m.FIdType == id);
            if (pharmacyTTypeList == null)
            {
                return NotFound();
            }

            return View(pharmacyTTypeList);
        }

        // GET: Drugs/PharmacyTTypeLists1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drugs/PharmacyTTypeLists1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FIdType,FTypeCode,FType")] PharmacyTTypeList pharmacyTTypeList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pharmacyTTypeList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pharmacyTTypeList);
        }

        // GET: Drugs/PharmacyTTypeLists1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PharmacyTTypeList == null)
            {
                return NotFound();
            }

            var pharmacyTTypeList = await _context.PharmacyTTypeList.FindAsync(id);
            if (pharmacyTTypeList == null)
            {
                return NotFound();
            }
            return View(pharmacyTTypeList);
        }

        // POST: Drugs/PharmacyTTypeLists1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FIdType,FTypeCode,FType")] PharmacyTTypeList pharmacyTTypeList)
        {
            if (id != pharmacyTTypeList.FIdType)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pharmacyTTypeList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PharmacyTTypeListExists(pharmacyTTypeList.FIdType))
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
            return View(pharmacyTTypeList);
        }

        // GET: Drugs/PharmacyTTypeLists1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PharmacyTTypeList == null)
            {
                return NotFound();
            }

            var pharmacyTTypeList = await _context.PharmacyTTypeList
                .FirstOrDefaultAsync(m => m.FIdType == id);
            if (pharmacyTTypeList == null)
            {
                return NotFound();
            }

            return View(pharmacyTTypeList);
        }

        // POST: Drugs/PharmacyTTypeLists1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PharmacyTTypeList == null)
            {
                return Problem("Entity set 'ClinicSysContext.PharmacyTTypeList'  is null.");
            }
            var pharmacyTTypeList = await _context.PharmacyTTypeList.FindAsync(id);
            if (pharmacyTTypeList != null)
            {
                _context.PharmacyTTypeList.Remove(pharmacyTTypeList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PharmacyTTypeListExists(int id)
        {
          return (_context.PharmacyTTypeList?.Any(e => e.FIdType == id)).GetValueOrDefault();
        }
    }
}
