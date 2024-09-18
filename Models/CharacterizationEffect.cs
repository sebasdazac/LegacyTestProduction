using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class CharacterizationEffect
    {
        public long Id { get; set; }
        public long IdCharacterization { get; set; }
        public string Effect { get; set; } = string.Empty;

        public string EffectType { get; set; } = string.Empty;


        public virtual CriterionCharacterization? IdCharacterizationNavigation { get; set; }
    }
}
