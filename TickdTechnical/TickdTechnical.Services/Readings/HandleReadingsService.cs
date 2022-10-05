using TickdTechnical.Models.Results;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using TickdTechnical.Service.Interfaces;

namespace TickdTechnical.Services
{
    public class HandleReadingsService : IHandleReadingsService
    {
        private IValidationService validationService;
        private ISubmitReadingService submitReadingService;

        public HandleReadingsService(IValidationService _validationService, ISubmitReadingService _submitReadingService)
        {
            validationService = _validationService;
            submitReadingService = _submitReadingService;
        }
        public async Task<Results> HandleReadings(IFormFile file)
        {
            Results results = new Results();

            StreamReader streamReader = new StreamReader(file.OpenReadStream());
            using (StreamReader reader = streamReader)
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (validationService.ValidateReading(values))
                    {
                        if (await submitReadingService.SubmitReading(values))
                        {
                            results.SuccessfulEntries += 1;
                        }
                        else
                        {
                            results.FailedEntries += 1;
                        }
                    }
                    else
                    {
                        results.FailedEntries += 1;
                    }
                }
            }

            return results;
        }
    }
}
