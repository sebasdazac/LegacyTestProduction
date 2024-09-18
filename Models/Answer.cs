using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class Answer
    {
        public long Id { get; set; }
        public long IdQuestion { get; set; }
        public string AnswerText { get; set; } = null!;
        public double Value { get; set; }
        public bool? IsActive { get; set; }

        public virtual Question IdQuestionNavigation { get; set; } = null!;
    }
}
