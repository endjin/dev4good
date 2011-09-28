namespace CraftAndDesignCouncil.Domain.Contracts.Tasks
{
    public interface IApplicationFormSectionTasks
    {
        ApplicationFormSection GetNextRequiredSectionForApplicationForm(int applicationFormId);
        ApplicationFormSection Get(int id);
    }
}