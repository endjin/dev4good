
namespace CraftAndDesignCouncil.Infrastructure.Queries
{
    #region Using Directives
    using CraftAndDesignCouncil.Domain.Contracts.Queries;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using CraftAndDesignCouncil.Domain;
    using SharpArch.NHibernate;
    using NHibernate.Linq;
    using System.Linq;
    #endregion

    public class GetSectionByIdQuery : NHibernateQuery<ApplicationFormSection>,IGetSectionByIdQuery
    {
        [Required(ErrorMessage="You must provide the Id when querying for a section")]
        public int sectionId { get; set; }

        public override IList<ApplicationFormSection> ExecuteQuery()
        {
            return Session.Query<ApplicationFormSection>().Where(x => x.Id == sectionId).ToList();
        }
    }
}
