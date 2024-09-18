using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class ViewCriterionClasification
    {
        public long Id { get; set; }
        public string CriterionText1 { get; set; } = null!;
        public string Clasification1 { get; set; } = null!;
        public long? Expr1 { get; set; }
        public string? CriterionText2 { get; set; }
        public string? Clasification2 { get; set; }
        public string Characterization { get; set; } = null!;
    }
}
