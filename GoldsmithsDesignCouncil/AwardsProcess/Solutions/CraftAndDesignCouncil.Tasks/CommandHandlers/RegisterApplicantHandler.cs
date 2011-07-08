namespace CraftAndDesignCouncil.Tasks.CommandHandlers
{
    #region Using Directives
    using CraftAndDesignCouncil.Tasks.Commands;
    using SharpArch.Domain.Commands;
using CraftAndDesignCouncil.Domain;
using SharpArch.NHibernate.Contracts.Repositories;
    using System;
    #endregion

    public class RegisterApplicantHandler : ICommandHandler<RegisterApplicantCommand>
    {
        private readonly INHibernateRepository<Applicant> repository;

        public RegisterApplicantHandler(INHibernateRepository<Applicant> repository)
        {
            this.repository = repository;
        }

        public ICommandResult Handle(RegisterApplicantCommand command)
        {
            command.Applicant.ModifiedDate = DateTime.Now;
            Applicant res = repository.Save(command.Applicant);

            return new RegisterApplicantResult(true, res.Id);
        }
    }
}