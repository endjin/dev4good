using CraftAndDesignCouncil.Domain;
using SharpArch.Domain.Commands;

namespace CraftAndDesignCouncil.Tasks.Commands
{
    public class StartNewApplicationFormCommand : CommandBase
    {
        private readonly Applicant applicant;

        public StartNewApplicationFormCommand(Applicant applicant)
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