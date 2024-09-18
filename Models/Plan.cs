using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class Plan
    {
        public Plan()
        {
            FormPlans = new HashSet<FormPlan>();
            PlanCompanies = new HashSet<PlanCompany>();
        }

        public long Id { get; set; }
        public string NamePlan { get; set; } = null!;
        public double Price { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
        public string? UserAccount { get; set; }
        public string? Forms { get; set; }
        public string? Bonus { get; set; }
        public int? Sort { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<FormPlan> FormPlans { get; set; }
        public virtual ICollection<PlanCompany> PlanCompanies { get; set; }
    }
}
