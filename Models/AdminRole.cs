using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class AdminRole
    {
        public AdminRole()
        {
            AdminPermissions = new HashSet<AdminPermission>();
            People = new HashSet<Person>();
        }

        public long Id { get; set; }
        public string Role { get; set; } = null!;

        public virtual ICollection<AdminPermission> AdminPermissions { get; set; }
        public virtual ICollection<Person> People { get; set; }
    }
}
