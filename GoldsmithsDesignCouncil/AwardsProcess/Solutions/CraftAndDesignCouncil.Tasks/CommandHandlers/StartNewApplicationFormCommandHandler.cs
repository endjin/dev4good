namespace CraftAndDesignCouncil.Tasks.CommandHandlers
{
    using CraftAndDesignCouncil.Tasks.Commands;
    using SharpArch.Domain.Commands;
    using CraftAndDesignCouncil.Domain;
    using SharpArch.NHibernate.Contracts.Repositories;

    public class StartNewApplicationFormCommandHandler : ICommandHandler<StartNewApplicationFormCommand>
    {
        INHibernateRepository<Applicant> applicantRepository;
        INHibernateRepository<ApplicationForm> applicationFormRepository;

        public StartNewApplicationFormCommandHandler(INHibernateRepository<Applicant> applicantRepository, INHibernateRepository<ApplicationForm> applicationFormRepository)
        {
            this.applicationFormRepository = applicationFormRepository;
            this.applicantRepository = applicantRepository;
        }

        public ICommandResult Handle(StartNewApplicationFormCommand command)
        {
            Applicant applicant = applicantRepository.Get(command.Applicant.Id);

            ApplicationForm form = new ApplicationForm();
            applicant.Applications.Add(form);
            
            applicantRepository.DbContext.CommitChanges();

            return new ApplicationFormResult(true, form.Id);
        }
    }
}