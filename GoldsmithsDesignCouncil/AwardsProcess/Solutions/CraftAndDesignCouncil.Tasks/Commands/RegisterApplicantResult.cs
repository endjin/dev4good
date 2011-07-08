namespace CraftAndDesignCouncil.Tasks.Commands
{
    using SharpArch.Domain.Commands;

    public class RegisterApplicantResult : CommandResult
    {
        private readonly int applicantId;

        public RegisterApplicantResult(bool success, int applicantId) : base(success)
        {
            this.applicantId = applicantId;
        }

        public int ApplicantId
        {
            get
            {
                return applicantId;
            }
        }
    }
}