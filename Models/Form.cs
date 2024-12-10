using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LegacyTest.Models
{
    public partial class Form
    {
        public Form()
        {
            Criteria = new HashSet<Criterion>();
            FormPlans = new HashSet<FormPlan>();
            ReportScales = new HashSet<ReportScale>();
        }

        public long Id { get; set; }
        public string NameForm { get; set; } = string.Empty;
        public string DescriptionReport { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? Alert { get; set; } = string.Empty;

        public string? Info { get; set; } = string.Empty;

        public virtual ICollection<Criterion>? Criteria { get; set; }
        public virtual ICollection<FormPlan>? FormPlans { get; set; }
        public virtual ICollection<ReportScale>? ReportScales { get; set; }
    }

}
