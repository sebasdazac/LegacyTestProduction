using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class Session
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public long IdPlanCompany { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string? Stated { get; set; }
    }
}
