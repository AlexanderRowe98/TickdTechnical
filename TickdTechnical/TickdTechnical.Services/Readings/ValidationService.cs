using System.Text.RegularExpressions;
using TickdTechnical.Service.Interfaces;
using TickdTechnical.Models.Readings;
using System.Linq;
using System;

namespace TickdTechnical.Services
{
    public class ValidationService : IValidationService
    {
        private TickdContext _context;

        public ValidationService(TickdContext context)
        {
            _context = context;
        }

        public bool ValidateReading(string[] reading)
        {
            bool isValidReading = false;

            if (ValidateValues(reading))
            {
                if (int.TryParse(reading[0], out int accountId))
                {
                    if (AccountExists(accountId))
                    {
                        if (!IsDuplicateReading(accountId, DateTime.Parse(reading[1]), reading[2]))
                        {
                            if (!IsOlderThanPreviousReadings(accountId, DateTime.Parse(reading[1])))
                            {
                                isValidReading = true;
                            }
                        }
                    }
                }                
            }

            return isValidReading;
        }

        private bool ValidateValues(string[] values)
        {
            if (!Regex.IsMatch(values[2], @"^\d{1,5}$") && !string.IsNullOrEmpty(values[0]))
            {
                return false;
            }

            return true;
        }

        private bool AccountExists(int id)
        {
            return _context.TblAccounts.Any(e => e.AccountId == id);
        }

        private bool IsDuplicateReading(int accountId, DateTime readingDate, string readingValue)
        {
            return _context.TblMeterReadings.Any(e => (e.AccountId == accountId) && (e.MeterReadingDatetime == readingDate) && (e.MeterReadValue == readingValue));
        }

        private bool IsOlderThanPreviousReadings(int accountId, DateTime readingDate)
        {
            return _context.TblMeterReadings.Any(e => (e.AccountId == accountId) && (e.MeterReadingDatetime > readingDate));
        }
    }
}
