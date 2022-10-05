using System;

namespace TickdTechnical.Models.Readings
{
    public partial class TblMeterReadings
    {
        public int EntryId { get; set; }
        public int AccountId { get; set; }
        public DateTime MeterReadingDatetime { get; set; }
        public string MeterReadValue { get; set; }
    }
}
