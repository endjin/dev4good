namespace CraftAndDesignCouncil.Domain
{
    using SharpArch.Domain.DomainModel;
    using System.Collections.Generic;
    using System;

    public class ApplicationForm : Entity
    {
        public virtual DateTime StartedOn { get; set; }
        public virtual IList<QuestionAnswer> Answers { get; set; }

        public ApplicationForm()
        {
            StartedOn = DateTime.Now;
        }
    }
}