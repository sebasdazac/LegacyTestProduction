using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class FormPlan
    {
        public long Id { get; set; }
        public long IdForm { get; set; }
        public long IdPlan { get; set; }
        public bool? IsActive { get; set; }

        public virtual Form IdFormNavigation { get; set; } = null!;
        public virtual Plan IdPlanNavigation { get; set; } = null!;
    }
}
