using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class AdminPermission
    {
        public long Id { get; set; }
        public long IdRole { get; set; }
        public long IdOperation { get; set; }

        public virtual AdminOperation IdOperationNavigation { get; set; } = null!;
        public virtual AdminRole IdRoleNavigation { get; set; } = null!;
    }
}
