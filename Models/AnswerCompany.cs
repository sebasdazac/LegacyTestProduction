using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class AnswerCompany
    {
        public long Id { get; set; }
        public long IdCompany { get; set; }
        public long IdPlanCompany { get; set; }
        public long IdCriterion { get; set; }
        public long IdQuestion { get; set; }
        public double AverageQuestion { get; set; }
        public DateTime Date { get; set; }

        public virtual Question? IdQuestionNavigation { get; set; } = null!;

    }
}
