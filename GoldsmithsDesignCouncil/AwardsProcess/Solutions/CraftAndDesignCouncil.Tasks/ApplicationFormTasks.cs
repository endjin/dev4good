namespace CraftAndDesignCouncil.Tasks
{
    #region using directives
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpArch.NHibernate.Contracts.Repositories;
    using CraftAndDesignCouncil.Domain;
    using CraftAndDesignCouncil.Domain.Contracts.Tasks;
    #endregion

    public class ApplicationFormTasks : IApplicationFormTasks
    {
        INHibernateRepository<Applicant> applicantRepository;
        INHibernateRepository<ApplicationForm> applicationFormRepository;

        public ApplicationFormTasks(INHibernateRepository<Applicant> applicantRepository, INHibernateRepository<ApplicationForm> applicationFormRepository)
        {
            this.applicationFormRepository = applicationFormRepository;
            this.applicantRepository = applicantRepository;
        }

        public ApplicationForm Get(int id)
        {
            return applicationFormRepository.Get(id);
        }
    }
}
