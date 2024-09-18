using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class Dimension
    {
        public Dimension()
        {
            Questions = new HashSet<Question>();
        }

        public long Id { get; set; }
        public string DimensionText { get; set; } = null!;
        public bool IsActive { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
