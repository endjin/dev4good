namespace CraftAndDesignCouncil.Specifications
{
    #region Using Directives
    using CraftAndDesignCouncil.Tasks;
    using Machine.Specifications.AutoMocking.Rhino;
    using Machine.Specifications;
    using System.Collections.Generic;
    using CraftAndDesignCouncil.Domain;
    using SharpArch.NHibernate.Contracts.Repositories;
    using Rhino.Mocks;
    #endregion

    public abstract class context_for_QuestionTasks : Specification<QuestionTasks>
    {
        Establish context = () =>
        {
            List<ApplicationFormSection> mockFormSectionData = new List<ApplicationFormSection>();
            for (int x = 1; x < 11; x++)
            {
                ApplicationFormSection section = new ApplicationFormSection { Title = "Section " + x };
                section.Questions = new List<Question>();
                section.Questions.Add(new Question { QuestionText = section.Title + "-Question 1" });
                section.Questions.Add(new Question { QuestionText = section.Title + "-Question 2" });
                mockFormSectionData.Add(section);
            }
            var mockFormSectionRepo = DependencyOf<INHibernateRepository<ApplicationFormSection>>();
            mockFormSectionRepo.Stub(x => x.PerformQuery(null)).IgnoreArguments().Return(mockFormSectionData);
        };
    }
}