namespace CraftAndDesignCouncil.Tasks.Commands
{
    using SharpArch.Domain.Commands;

    public class ApplicantResult : CommandResult
    {
        private readonly int applicantId;

        public ApplicantResult(bool success, int applicantId) : base(success)
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