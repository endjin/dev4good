namespace CraftAndDesignCouncil.Tasks.CommandHandlers
{
    using SharpArch.Domain.Commands;
    using CraftAndDesignCouncil.Tasks.Commands;
    using CraftAndDesignCouncil.Domain;
    using SharpArch.NHibernate.Contracts.Repositories;

    public class StartNewApplicationFormCommandHandler : ICommandHandler<StartNewApplicationFormCommand>
    {
        private readonly INHibernateRepository<Applicant> applicantRepository;
        private Applicant ApplicantStartingForm { get; set; }

        public StartNewApplicationFormCommandHandler(INHibernateRepository<Applicant> applicantRepository)
        {
            this.applicantRepository = applicantRepository;
        }

        public ICommandResult Handle(StartNewApplicationFormCommand command)
        {
            Applicant applicantFromRepo = applicantRepository.Get(command.ApplicantId);

            ApplicationForm form = new ApplicationForm();
            applicantFromRepo.Applications.Add(form);
            applicantRepository.DbContext.CommitChanges();

            return new ApplicationFormResult(true,form.Id);
        }
    }
}