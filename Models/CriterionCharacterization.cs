using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LegacyTest.Models
{
    public partial class CriterionCharacterization
    {
        public CriterionCharacterization()
        {
            CharacterizationByCompanies = new HashSet<CharacterizationByCompany>();
            CharacterizationEffects = new HashSet<CharacterizationEffect>();
            CharacterizationRecomendations = new HashSet<CharacterizationRecomendation>();
        }

        public long Id { get; set; }
        public long IdCriterion1 { get; set; }
        public string Clasification1 { get; set; } = null!;
        public double Min1 { get; set; }
        public double Max1 { get; set; }
        public long? IdCriterion2 { get; set; }
        public string? Clasification2 { get; set; }
        public double? Min2 { get; set; }
        public double? Max2 { get; set; }
        public string Characterization { get; set; } = null!;

        [JsonIgnore]
        public virtual Criterion? IdCriterion1Navigation { get; set; } = null!;
        public virtual Criterion? IdCriterion2Navigation { get; set; }

        public virtual ICollection<CharacterizationByCompany>? CharacterizationByCompanies { get; set; }
        public virtual ICollection<CharacterizationEffect>? CharacterizationEffects { get; set; }
        public virtual ICollection<CharacterizationRecomendation>? CharacterizationRecomendations { get; set; }

    }
}
