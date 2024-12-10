using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class PlanCompany
    {
        public PlanCompany()
        {
            AnswerCriterionCompanies = new HashSet<AnswerCriterionCompany>();
        }

        public long Id { get; set; }
        public long IdCompany { get; set; }
        public long IdPlan { get; set; }
        public DateTime DateInitial { get; set; }
        public DateTime DateEnd { get; set; }
        public bool? IsActive { get; set; }

        public virtual Company? IdCompanyNavigation { get; set; } 
        public virtual Plan? IdPlanNavigation { get; set; } 
        public virtual ICollection<AnswerCriterionCompany>? AnswerCriterionCompanies { get; set; }
    }
}
