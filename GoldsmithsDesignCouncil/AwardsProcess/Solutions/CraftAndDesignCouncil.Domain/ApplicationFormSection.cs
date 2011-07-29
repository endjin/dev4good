namespace CraftAndDesignCouncil.Domain
{
    #region Using Directives
    using SharpArch.Domain.DomainModel;
    using System.Collections.Generic;
    #endregion

    public class ApplicationFormSection : Entity
    {
       public virtual string Title { get; set; }
       public virtual int OrderingKey { get; set; }
       public virtual IList<QuestionAnswer> Questions { get; set; }
       public virtual Question NotRequiredIfQuestion { get; set; }
       public virtual string NotRequiredIfAnswer { get; set; }
    }
}