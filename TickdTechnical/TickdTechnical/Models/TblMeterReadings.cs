using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TickdTechnical.Models
{
    public partial class TblMeterReadings
    {
        public int EntryId { get; set; }
        public int AccountId { get; set; }
        public DateTime MeterReadingDatetime { get; set; }
        public string MeterReadValue { get; set; }
    }
}
