using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TickdTechnical.Models;

namespace TickdTechnical.Controllers
{
    [Route("api/meter-reading-uploads")]
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
        public async Task<IActionResult> PostTblMeterReading([FromForm] IFormFile file)
        {
            try
            {
                int successfullEntries = 0;
                int failedEntries = 0;
                int alreadyExists = 0;

                // Initialise StreamReader to read through the uploaded CSV file
                using (StreamReader reader = new StreamReader(file.OpenReadStream()))
                {
                    // Ignore first line (headings)
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        // Regex to make sure only numbers are present in the meter reading field / to format of NNNNN
                        // Ensure account id field in csv has data
                        if (Regex.IsMatch(values[2], @"^\d{5}$") && !string.IsNullOrEmpty(values[0]))
                        {
                            if (int.TryParse(values[0], out int accountId))
                            {
                                // Check to see if there is a valid account associated to the meter reading
                                TblAccounts account = await _context.TblAccounts.FirstOrDefaultAsync(x => x.AccountId == accountId);

                                if (account != null)
                                {

                                    // Check to see if there is an existing meter reading matching the data
                                    TblMeterReadings existingReading = await _context.TblMeterReadings.FirstOrDefaultAsync(x => (x.AccountId == accountId)
                                        && (x.MeterReadingDatetime == DateTime.Parse(values[1]))
                                        && (x.MeterReadValue == values[2]));

                                    // Proceed if no existing reading
                                    if (existingReading == null)
                                    {
                                        // Initialise TblMeterReading class and assign values from stream reader
                                        TblMeterReadings meterReading = new TblMeterReadings
                                        {
                                            AccountId = accountId,
                                            MeterReadingDatetime = DateTime.Parse(values[1]),
                                            MeterReadValue = values[2]
                                        };

                                        // Add meter reading to entity state ready to be inserted into the DB
                                        _context.TblMeterReadings.Add(meterReading);
                                        successfullEntries += 1;
                                    }
                                    else
                                    {
                                        // Meter reading entry already exists
                                        alreadyExists += 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Entry of meter reading failed due to being invalid
                            failedEntries += 1;
                        }
                    }
                }

                // Insert all new meter readings from entity state
                await _context.SaveChangesAsync();

                // Return Success and provide number of successful, failed and duplicate entries from the file that was uploaded
                return Ok(new { successfullEntries, failedEntries, alreadyExists });
            }
            catch (Exception)
            {
                throw new Exception();
            }
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
