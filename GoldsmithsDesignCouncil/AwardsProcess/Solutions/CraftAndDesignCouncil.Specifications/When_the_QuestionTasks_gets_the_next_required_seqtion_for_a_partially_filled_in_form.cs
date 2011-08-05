namespace CraftAndDesignCouncil.Specifications
{
    using Machine.Specifications;
    using CraftAndDesignCouncil.Domain;

    public class When_the_QuestionTasks_gets_the_next_required_section : context_for_QuestionTasks
    {
        static ApplicationFormSection result;

        Because of = () => result = subject.GetNextRequiredSectionForApplicationForm(2);

        It should_skip_completed_sections = () => result.Title.ShouldNotEqual("Section 1");

        It should_skip_non_required_sections = () => result.Title.ShouldNotEqual("Section 2");

        It should_not_skip_partially_complete_sections = () => result.Title.ShouldEqual("Section 3");
    }
}