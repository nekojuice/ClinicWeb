using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicWeb.Models;

namespace ClinicWeb.Areas.Cases.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CAController : ControllerBase
    {
        private readonly ClinicSysContext _context;

        public CAController(ClinicSysContext context)
        {
            _context = context;
        }

        // GET: api/CA
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CasesMainCase>>> GetCasesMainCase()
        {
          if (_context.CasesMainCase == null)
          {
              return NotFound();
          }
            return await _context.CasesMainCase.ToListAsync();
        }

        // GET: api/CA/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CasesMainCase>> GetCasesMainCase(int id)
        {
          if (_context.CasesMainCase == null)
          {
              return NotFound();
          }
            var casesMainCase = await _context.CasesMainCase.FindAsync(id);

            if (casesMainCase == null)
            {
                return NotFound();
            }

            return casesMainCase;
        }

        // PUT: api/CA/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCasesMainCase(int id, CasesMainCase casesMainCase)
        {
            if (id != casesMainCase.CaseId)
            {
                return BadRequest();
            }

            _context.Entry(casesMainCase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CasesMainCaseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CA
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CasesMainCase>> PostCasesMainCase(CasesMainCase casesMainCase)
        {
          if (_context.CasesMainCase == null)
          {
              return Problem("Entity set 'ClinicSysContext.CasesMainCase'  is null.");
          }
            _context.CasesMainCase.Add(casesMainCase);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCasesMainCase", new { id = casesMainCase.CaseId }, casesMainCase);
        }

        // DELETE: api/CA/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCasesMainCase(int id)
        {
            if (_context.CasesMainCase == null)
            {
                return NotFound();
            }
            var casesMainCase = await _context.CasesMainCase.FindAsync(id);
            if (casesMainCase == null)
            {
                return NotFound();
            }

            _context.CasesMainCase.Remove(casesMainCase);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CasesMainCaseExists(int id)
        {
            return (_context.CasesMainCase?.Any(e => e.CaseId == id)).GetValueOrDefault();
        }
    }
}
