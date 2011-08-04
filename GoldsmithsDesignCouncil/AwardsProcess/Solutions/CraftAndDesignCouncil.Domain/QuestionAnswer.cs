using SharpArch.Domain.DomainModel;

namespace CraftAndDesignCouncil.Domain
{
    public class QuestionAnswer : Entity
    {
        public virtual string AnswerText { get; set; }
        public virtual Question Question { get; set; }
    }
}