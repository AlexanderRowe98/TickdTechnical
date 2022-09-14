using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TickdTechnical.Models;

namespace TickdTechnical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TblMeterReadingsController : ControllerBase
    {
        private readonly TickdContext _context;

        public TblMeterReadingsController(TickdContext context)
        {
            _context = context;
        }

        // GET: api/TblMeterReadings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblMeterReadings>>> GetTblMeterReadings()
        {
            return await _context.TblMeterReadings.ToListAsync();
        }

        // GET: api/TblMeterReadings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblMeterReadings>> GetTblMeterReadings(int id)
        {
            var tblMeterReadings = await _context.TblMeterReadings.FindAsync(id);

            if (tblMeterReadings == null)
            {
                return NotFound();
            }

            return tblMeterReadings;
        }

        // PUT: api/TblMeterReadings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblMeterReadings(int id, TblMeterReadings tblMeterReadings)
        {
            if (id != tblMeterReadings.EntryId)
            {
                return BadRequest();
            }

            _context.Entry(tblMeterReadings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblMeterReadingsExists(id))
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

        // POST: api/TblMeterReadings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TblMeterReadings>> PostTblMeterReadings(TblMeterReadings tblMeterReadings)
        {
            _context.TblMeterReadings.Add(tblMeterReadings);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TblMeterReadingsExists(tblMeterReadings.EntryId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTblMeterReadings", new { id = tblMeterReadings.EntryId }, tblMeterReadings);
        }

        // DELETE: api/TblMeterReadings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TblMeterReadings>> DeleteTblMeterReadings(int id)
        {
            var tblMeterReadings = await _context.TblMeterReadings.FindAsync(id);
            if (tblMeterReadings == null)
            {
                return NotFound();
            }

            _context.TblMeterReadings.Remove(tblMeterReadings);
            await _context.SaveChangesAsync();

            return tblMeterReadings;
        }

        private bool TblMeterReadingsExists(int id)
        {
            return _context.TblMeterReadings.Any(e => e.EntryId == id);
        }
    }
}
