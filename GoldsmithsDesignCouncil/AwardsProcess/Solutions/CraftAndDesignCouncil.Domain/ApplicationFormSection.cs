namespace CraftAndDesignCouncil.Domain
{
    #region Using Directives
    using SharpArch.Domain.DomainModel;
    using System.Collections.Generic;
    #endregion

    public class ApplicationFormSection : Entity
    {
        public virtual IList<QuestionAnswer> Questions { get; set; }
    }
}