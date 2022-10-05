using System;
using System.Threading.Tasks;
using TickdTechnical.Models.Readings;
using TickdTechnical.Service.Interfaces;

namespace TickdTechnical.Services
{
    public class SubmitReadingService : ISubmitReadingService
    {
        private TickdContext _context;

        public SubmitReadingService(TickdContext context)
        {
            _context = context;
        }
        public async Task<bool> SubmitReading(string[] values)
        {
            try
            {
                if (int.TryParse(values[0], out int accountId))
                {
                    TblMeterReadings meterReading = new TblMeterReadings
                    {
                        AccountId = accountId,
                        MeterReadingDatetime = DateTime.Parse(values[1]),
                        MeterReadValue = values[2].PadLeft(5, '0')
                    };

                    await InsertData(meterReading);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }            
        }

        private async Task InsertData(TblMeterReadings meterReading)
        {
            _context.TblMeterReadings.Add(meterReading);

            await _context.SaveChangesAsync();
        }
    }
}
