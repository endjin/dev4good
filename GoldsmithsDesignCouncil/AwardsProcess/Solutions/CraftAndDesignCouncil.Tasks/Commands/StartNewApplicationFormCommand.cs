namespace CraftAndDesignCouncil.Tasks.Commands
{
    using SharpArch.Domain.Commands;
    using CraftAndDesignCouncil.Domain;

    public class StartNewApplicationFormCommand : CommandBase
    {
        public Applicant Applicant { get; set; }
        public int ApplicantId {get{return Applicant.Id;}}
    }
}