namespace CraftAndDesignCouncil.Domain
{
    using SharpArch.Domain.DomainModel;
using System.Collections.Generic;

    public class ApplicationForm : Entity
    {
        public virtual IList<ApplicationFormSection> Sections { get; set; }
    }
}