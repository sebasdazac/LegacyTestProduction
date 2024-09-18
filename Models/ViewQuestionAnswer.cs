using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class ViewQuestionAnswer
    {
        public long IdForm { get; set; }
        public long IdQuestion { get; set; }
        public long IdAnswer { get; set; }
        public long IdCompany { get; set; }
        public long IdPlanCompany { get; set; }
        public long IdPerson { get; set; }
        public long IdCriterio { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
}
