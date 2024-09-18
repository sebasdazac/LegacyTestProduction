using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class AdminModule
    {
        public AdminModule()
        {
            AdminOperations = new HashSet<AdminOperation>();
        }

        public long Id { get; set; }
        public string Module { get; set; } = null!;

        public virtual ICollection<AdminOperation> AdminOperations { get; set; }
    }
}
