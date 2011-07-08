namespace CraftAndDesignCouncil.Tasks.CommandHandlers
{
    using SharpArch.Domain.Commands;

    public class RegisterApplicantResult : CommandResult
    {
        public RegisterApplicantResult(bool success) : base(success)
        {
        }
    }
}