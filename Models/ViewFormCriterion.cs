using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class ViewFormCriterion
    {

        public long Id { get; set; }
        public string CriterionText { get; set; } = null!;
        public long IdForm { get; set; }

        public string? NameForm { get; set; }
        public long IdCriterion { get; set; }

        public long IdQuestion { get; set; }
        public string QuestionText { get; set; } = null!;

    }
}
