using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LegacyTest.Models
{
    public partial class ReportScale
    {
        public long Id { get; set; }

        public long IdForm { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }


        [JsonIgnore]
        public virtual Form? IdFormNavigation { get; set; }
    }
}
