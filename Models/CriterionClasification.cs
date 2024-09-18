using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class CriterionClasification
    {
        public long Id { get; set; }
        public long IdCriterion { get; set; }
        public string Clasification { get; set; } = null!;
        public double Min { get; set; }
        public double Max { get; set; }

        public virtual Criterion IdCriterionNavigation { get; set; } = null!;
    }
}
