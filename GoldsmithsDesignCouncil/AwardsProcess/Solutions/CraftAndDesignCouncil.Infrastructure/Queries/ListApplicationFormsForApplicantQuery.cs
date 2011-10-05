namespace CraftAndDesignCouncil.Infrastructure.Queries
{
    #region Using Directives
    using CraftAndDesignCouncil.Domain;
    using System.Collections.Generic;
    using System.Linq;
    using CraftAndDesignCouncil.Domain.Contracts.Queries;
    using SharpArch.NHibernate;
    using NHibernate.Linq;
    using System.ComponentModel.DataAnnotations;
    #endregion

    public class ListApplicationFormsForApplicantQuery : NHibernateQuery<ApplicationForm>,IListApplicationFormsForApplicantQuery
    {
        [Required(ErrorMessage="You must provide the applicant when querying for their forms")]
        public Applicant Applicant { get; set; }

        public override IList<ApplicationForm> ExecuteQuery()
        {
            var results = Session.Query<Applicant>()
                                .Where(x => x == Applicant)
                                .SelectMany(x => x.Applications);

            return results.ToList();
            
        }
    }
}
