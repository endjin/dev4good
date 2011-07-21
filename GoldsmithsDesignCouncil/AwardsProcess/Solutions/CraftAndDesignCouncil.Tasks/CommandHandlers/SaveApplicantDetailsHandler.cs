namespace CraftAndDesignCouncil.Tasks.CommandHandlers
{
    using CraftAndDesignCouncil.Tasks.Commands;
    using SharpArch.Domain.Commands;
    using CraftAndDesignCouncil.Domain;
    using SharpArch.NHibernate.Contracts.Repositories;
    using System;


    public class SaveApplicantDetailsHandler : ICommandHandler<SaveApplicantDetailsCommand>
    {
        private readonly INHibernateRepository<Applicant> applicantRepository;

        public SaveApplicantDetailsHandler(INHibernateRepository<Applicant> applicantRepository)
        {
            this.applicantRepository = applicantRepository;
        }

        public ICommandResult Handle(SaveApplicantDetailsCommand command)
        {
            Applicant entity = applicantRepository.Get(command.Applicant.Id);

            entity.ModifiedDate = DateTime.Now;
            entity.Email = command.Applicant.Email;
            entity.FirstName = command.Applicant.FirstName;
            entity.LastName = command.Applicant.LastName;

            applicantRepository.DbContext.CommitChanges();

            return new ApplicantResult(true, entity.Id);
        }
    }
}