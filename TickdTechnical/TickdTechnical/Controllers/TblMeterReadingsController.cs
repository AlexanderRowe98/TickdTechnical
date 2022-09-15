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

        // POST: api/meter-reading-uploads
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> PostTblMeterReading([FromForm] IFormFile file)
        {
            if (file == null)
                return BadRequest("No file was provided.");

            if (file.ContentType != "text/csv")
                return BadRequest("File was not in the correct format: .csv");

            // Variables to track the status of each entry
            int successfulEntries = 0;
            int failedEntries = 0;
            int duplicateEntries = 0;

            var entity = _context.TblMeterReadings;

            // Initialise StreamReader to read through the uploaded CSV file
            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            {
                // Ignore first line (headings)
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    //Check to see if the reading values are valid
                    if (TblMeterReadingsIsValid(values))
                    {
                        if (int.TryParse(values[0], out int accountId))
                        {
                            // Check to see if reading values account ID is a valid account in tbl_accounts
                            if (AccountExists(accountId))
                            {
                                // Check to see if this particular reading is already in the database / is a duplicate
                                if (!IsDuplicateReading(accountId, DateTime.Parse(values[1]), values[2]))
                                {
                                    // Check to see if there is previous meter reading for this account
                                    List<TblMeterReadings> previousReadings = await _context.TblMeterReadings.Where(x => x.AccountId == accountId).ToListAsync();
                                    bool isOlder = false;

                                    // If there are previous readings relating to this account Id, loop through each of them 
                                    // Check if previous reading Datetime is more recent than the entry in the .csv file
                                    if (previousReadings.Count > 0)
                                    {
                                        foreach (var reading in previousReadings)
                                        {
                                            if (isOlder == false && reading.MeterReadingDatetime > DateTime.Parse(values[1]))
                                            {
                                                isOlder = true;
                                                break;
                                            }
                                        }
                                    }                                    

                                    // If new record is not older than previous records
                                    if (!isOlder)
                                    {
                                        // Initialise TblMeterReading class and assign values from stream reader
                                        TblMeterReadings meterReading = new TblMeterReadings
                                        {
                                            AccountId = accountId,
                                            MeterReadingDatetime = DateTime.Parse(values[1]),
                                            MeterReadValue = values[2]
                                        };

                                        // Add meter reading to entity state ready to be inserted into the DB
                                        entity.Add(meterReading);
                                        successfulEntries += 1;
                                    }
                                    else
                                    {
                                        // Entry of meter reading failed due to being invalid
                                        failedEntries += 1;
                                    }
                                }
                                else
                                {
                                    // Entry of meter reading not submitted due to being a duplicate
                                    duplicateEntries += 1;
                                }
                            }
                            else
                            {
                                // Entry of meter reading failed due to no valid account
                                failedEntries += 1;
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

            try
            {
                // Insert all new meter readings from entity state to DB
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            // Return Success and provide number of successful, failed and duplicate entries from the file that was uploaded
            return Ok(new { successfulEntries, failedEntries, duplicateEntries });
        }

        private bool TblMeterReadingsIsValid(string[] values)
        {
            if (Regex.IsMatch(values[2], @"^\d{5}$") && !string.IsNullOrEmpty(values[0]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool AccountExists(int id)
        {
            return _context.TblAccounts.Any(e => e.AccountId == id);
        }

        private bool IsDuplicateReading (int accountId, DateTime readingDate, string readingValue)
        {
            return _context.TblMeterReadings.Any(e => (e.AccountId == accountId) && (e.MeterReadingDatetime == readingDate) && (e.MeterReadValue == readingValue));
        }
    }
}