using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class AnswerPerson
    {
        public long Id { get; set; }
        public long IdQuestion { get; set; }
        public long IdAnswer { get; set; }
        public long IdCompany { get; set; }
        public long IdPlanCompany { get; set; }
        public long IdPerson { get; set; }
        public long IdCriterio { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public double? AvgFatherSons { get; set; }

        public virtual Question? IdQuestionNavigation { get; set; } = null!;
    }
}
