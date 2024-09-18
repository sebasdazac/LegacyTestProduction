using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class AnswerCriterionCompany
    {
        public long Id { get; set; }
        public long IdCompany { get; set; }
        public long IdPlanCompany { get; set; }
        public long IdCriterion { get; set; }
        public double AverageCriterion { get; set; }
        public DateTime Date { get; set; }

        public virtual Criterion IdCriterionNavigation { get; set; } = null!;
        public virtual PlanCompany IdPlanCompanyNavigation { get; set; } = null!;
    }
}
