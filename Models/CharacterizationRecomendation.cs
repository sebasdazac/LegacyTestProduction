using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LegacyTest.Models
{
    public partial class CharacterizationRecomendation
    {
        public long Id { get; set; }
        public long IdCharacterization { get; set; }
        public string Recomendation { get; set; } = string.Empty;
        public string RecomendationType { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual CriterionCharacterization IdCharacterizationNavigation { get; set; } = null!;
    }
}
