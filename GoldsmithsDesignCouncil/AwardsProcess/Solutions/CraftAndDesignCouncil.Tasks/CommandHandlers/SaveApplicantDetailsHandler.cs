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
        private readonly INHibernateRepository<Address> addressRepository;

        public SaveApplicantDetailsHandler(INHibernateRepository<Applicant> applicantRepository, INHibernateRepository<Address> addressRepository)
        {
            this.addressRepository = addressRepository;
            this.applicantRepository = applicantRepository;
        }

        public ICommandResult Handle(SaveApplicantDetailsCommand command)
        {
            Applicant applicant = applicantRepository.Get(command.Applicant.Id);

            applicant.ModifiedDate = DateTime.Now;
            applicant.Email = command.Applicant.Email;
            applicant.FirstName = command.Applicant.FirstName;
            applicant.LastName = command.Applicant.LastName;
            if (applicant.Address == null)
            {
                applicant.Address = command.Applicant.Address.CloneTo();
                applicant.Address.ModifiedDate = DateTime.Now;
                addressRepository.Save(applicant.Address);
            }
            else
            {
                applicant.Address.CopyAllPropertiesFrom(command.Applicant.Address);
                applicant.Address.ModifiedDate = DateTime.Now;
            }

            applicantRepository.DbContext.CommitChanges();

            return new ApplicantResult(true, applicant.Id);
        }
    }
}