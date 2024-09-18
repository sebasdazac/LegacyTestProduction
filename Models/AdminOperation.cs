using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class AdminOperation
    {
        public AdminOperation()
        {
            AdminPermissions = new HashSet<AdminPermission>();
        }

        public long Id { get; set; }
        public long IdModule { get; set; }
        public string Operation { get; set; } = null!;

        public virtual AdminModule IdModuleNavigation { get; set; } = null!;
        public virtual ICollection<AdminPermission> AdminPermissions { get; set; }
    }
}
