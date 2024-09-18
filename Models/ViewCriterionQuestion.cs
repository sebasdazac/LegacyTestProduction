using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class ViewCriterionQuestion
    {
        public long IdForm { get; set; }
        public string? NameForm { get; set; }
        public long IdCriterion { get; set; }
        public string CriterionText { get; set; } = null!;
        public long IdQuestion { get; set; }
        public string QuestionText { get; set; } = null!;
    }
}
