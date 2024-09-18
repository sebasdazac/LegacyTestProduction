using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Pswd { get; set; } = null!;
        public string? State { get; set; }
        public int RoleId { get; set; }
    }
}
