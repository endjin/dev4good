namespace CraftAndDesignCouncil.Domain
{
    #region Using Directives
    using SharpArch.Domain.DomainModel;
    #endregion

    public class Question : Entity
    {
        public virtual string LongText { get; set; }

        public virtual string HelpText { get; set; }
    }
}
