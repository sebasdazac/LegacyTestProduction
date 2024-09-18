using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class ViewFormClasification
    {
        public long IdForm { get; set; }
        public string? NameForm { get; set; }
        public long IdCriterion { get; set; }
        public string CriterionText { get; set; } = null!;
        public string Clasification { get; set; } = null!;
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
