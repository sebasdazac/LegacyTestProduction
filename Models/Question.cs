using System;
using System.Collections.Generic;

namespace LegacyTest.Models
{
    public partial class Question
    {
        public Question()
        {
            AnswerPeople = new HashSet<AnswerPerson>();
            AnswerCompanies = new HashSet<AnswerCompany>();
            Answers = new HashSet<Answer>();
            InverseQuestionParentNavigation = new HashSet<Question>();
        }

        public long Id { get; set; }
        public long IdCriterion { get; set; }
        public long? QuestionParent { get; set; }
        public string QuestionText { get; set; } = null!;
        public bool? IsActive { get; set; }
        public long? IdDimension { get; set; }

        public int? OrderBy { get; set; }

        public virtual Criterion IdCriterionNavigation { get; set; } = null!;
        public virtual Dimension? IdDimensionNavigation { get; set; }
        public virtual Question? QuestionParentNavigation { get; set; }
        public virtual ICollection<AnswerCompany> AnswerCompanies { get; set; }

        public virtual ICollection<AnswerPerson> AnswerPeople { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Question> InverseQuestionParentNavigation { get; set; }
    }
}
