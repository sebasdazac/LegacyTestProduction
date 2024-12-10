using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class AnswerPersonCount
    {
        public long Id { get; set; }
        public long IdPerson { get; set; }
        public long IdPlanCompany { get; set; }
        public long IdForm { get; set; }
        public DateTime SaveDate { get; set; }
        public long IdCompany { get; set; }
    }
}
