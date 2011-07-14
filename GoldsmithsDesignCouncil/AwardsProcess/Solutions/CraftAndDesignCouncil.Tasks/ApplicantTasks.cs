namespace CraftAndDesignCouncil.Tasks
{
    #region Using Directives
    using CraftAndDesignCouncil.Domain;
    using CraftAndDesignCouncil.Domain.Contracts.Tasks;
    using SharpArch.NHibernate.Contracts.Repositories;
    #endregion

    public class ApplicantTasks : IApplicantTasks
    {
        private readonly INHibernateRepository<Applicant> applicantRepository;

        public ApplicantTasks(INHibernateRepository<Applicant> applicantRepository)
        {
            this.applicantRepository = applicantRepository;
        }

        public Applicant Get(int id)
        {
            return applicantRepository.Get(id);
        }
    }
}
