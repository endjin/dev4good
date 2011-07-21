using SharpArch.Domain.Commands;
using CraftAndDesignCouncil.Domain;

namespace CraftAndDesignCouncil.Tasks.Commands
{
    public class RegisterApplicantCommand : CommandBase
    {
        private readonly Applicant applicant;

        public RegisterApplicantCommand(Applicant applicant)
        {
            this.applicant = applicant;
        }

        public Applicant Applicant
        {
            get
            {
                return applicant;
            }
        }
    }
}