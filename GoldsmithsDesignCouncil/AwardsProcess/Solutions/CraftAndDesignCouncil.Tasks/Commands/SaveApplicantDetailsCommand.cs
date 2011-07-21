using SharpArch.Domain.Commands;
using CraftAndDesignCouncil.Domain;

namespace CraftAndDesignCouncil.Tasks.Commands
{
    public class SaveApplicantDetailsCommand : CommandBase
    {
        private readonly Applicant applicant;

        public SaveApplicantDetailsCommand(Applicant applicant)
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