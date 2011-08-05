namespace CraftAndDesignCouncil.Specifications
{
    #region Using Directives
    using Machine.Specifications;
    using Machine.Specifications.AutoMocking.Rhino;
    using CraftAndDesignCouncil.Tasks;
    using CraftAndDesignCouncil.Domain;
    using System.Collections.Generic;
    using SharpArch.NHibernate.Contracts.Repositories;
    using Rhino.Mocks;
    #endregion

    public class When_the_QuestionTasks_gets_the_next_required_section_for_a_new_form : context_for_QuestionTasks
    {
        static ApplicationFormSection result;

        Because of =()=> result = subject.GetNextRequiredSectionForApplicationForm(1);

        It should_get_the_first_section =()=> result.Title.ShouldEqual("Section 1");
    }
}
