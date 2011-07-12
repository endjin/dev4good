using SharpArch.Domain.DomainModel;

namespace CraftAndDesignCouncil.Domain
{
    public class QuestionAnswer : Entity
    {
        public virtual string OriginalQuestionText { get; set; }
        public virtual string AnswerText { get; set; }
    }
}