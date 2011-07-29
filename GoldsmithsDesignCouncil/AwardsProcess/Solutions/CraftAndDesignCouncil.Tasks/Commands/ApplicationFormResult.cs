namespace CraftAndDesignCouncil.Tasks.Commands
{
    using SharpArch.Domain.Commands;

    public class ApplicationFormResult : CommandResult
    {
        private readonly int applicationFormId;

        public ApplicationFormResult(bool success, int applicationFormId)
        : base(success)
        {
            this.applicationFormId = applicationFormId;
        }

        public int ApplicationFormId
        {
            get
            {
                return applicationFormId;
            }
        }
    }
}