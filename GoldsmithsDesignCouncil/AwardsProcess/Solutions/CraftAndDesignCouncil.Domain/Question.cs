namespace CraftAndDesignCouncil.Domain
{
    #region Using Directives
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpArch.Domain.DomainModel;
    #endregion

    public class Question : Entity
    {
        public virtual string QuestionText { get; set; }
        public virtual string HelpText { get; set; }
    }
}
