using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class Company
    {
        public Company()
        {
            People = new HashSet<Person>();
            PlanCompanies = new HashSet<PlanCompany>();
        }

        public long Id { get; set; }
        public string BusinessName { get; set; } = null!;
        public string CommercialReg { get; set; } = null!;
        public string? TypeReg { get; set; }

        public virtual ICollection<Person> People { get; set; }
        public virtual ICollection<PlanCompany> PlanCompanies { get; set; }
    }
}
