using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class Criterion
    {
        public Criterion()
        {
            AnswerCriterionCompanies = new HashSet<AnswerCriterionCompany>();
            CriterionCharacterizationIdCriterion1Navigations = new HashSet<CriterionCharacterization>();
            CriterionCharacterizationIdCriterion2Navigations = new HashSet<CriterionCharacterization>();
            CriterionClasifications = new HashSet<CriterionClasification>();
            Questions = new HashSet<Question>();
        }

        public long Id { get; set; }
        public long IdForm { get; set; }
        public string CriterionText { get; set; } = null!;
        public string? Description { get; set; }

        public virtual Form IdFormNavigation { get; set; } = null!;
        public virtual ICollection<AnswerCriterionCompany> AnswerCriterionCompanies { get; set; }
        public virtual ICollection<CriterionCharacterization> CriterionCharacterizationIdCriterion1Navigations { get; set; }
        public virtual ICollection<CriterionCharacterization> CriterionCharacterizationIdCriterion2Navigations { get; set; }
        public virtual ICollection<CriterionClasification> CriterionClasifications { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
