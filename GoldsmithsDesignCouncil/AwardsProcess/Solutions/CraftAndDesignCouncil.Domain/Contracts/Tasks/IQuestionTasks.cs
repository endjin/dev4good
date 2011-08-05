namespace CraftAndDesignCouncil.Domain.Contracts.Tasks
{
    public interface IQuestionTasks
    {
        ApplicationFormSection GetNextRequiredSectionForApplicationForm(int applicationFormId);
    }
}