using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class Person
    {
        public long Id { get; set; }
        public long IdCompany { get; set; }
        public long IdRole { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Pswd { get; set; } = null!;
        public string? Token { get; set; }
        public string State { get; set; } = null!;
        public byte[]? Photo { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }


        public virtual Company IdCompanyNavigation { get; set; } = null!;
        public virtual AdminRole IdRoleNavigation { get; set; } = null!;
    }
}
