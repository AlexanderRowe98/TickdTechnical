using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TickdTechnical.Models.Results;
using TickdTechnical.Service.Interfaces;

namespace TickdTechnical.Controllers
{
    [Route("api/meter-reading-uploads")]
    [ApiController]
    public class ReadingsController : ControllerBase
    {
        private IHandleReadingsService _handleReadingsService;

        public ReadingsController(IHandleReadingsService handleReadingsService)
        {
            _handleReadingsService = handleReadingsService;
        }

        // POST: api/meter-reading-uploads
        [HttpPost]
        public async Task<IActionResult> PostTblMeterReading([FromForm] IFormFile file)
        {
            if (file == null)
                return BadRequest("No file was provided.");

            if (file.ContentType != "text/csv")
                return BadRequest("File was not in the correct format: .csv");

            try
            {
                Results results = await _handleReadingsService.HandleReadings(file);

                int successfulEntries = results.SuccessfulEntries;
                int failedEntries = results.FailedEntries;

                return Ok(new { successfulEntries, failedEntries });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "An error occurred when inserting the entries into the database. Please try again.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Oops something went wrong! Please try again and if this issue persists. please contact the administrator.");
            }
        }
    }
}