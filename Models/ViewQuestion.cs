using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class ViewQuestion
    {
        public long Id { get; set; }
        public long IdForm { get; set; }
        public string? NameForm { get; set; }
        public long? QuestionParent { get; set; }
        public string QuestionText { get; set; } = null!;
    }
}
