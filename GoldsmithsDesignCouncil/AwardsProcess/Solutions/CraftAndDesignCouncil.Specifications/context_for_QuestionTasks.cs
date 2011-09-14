namespace CraftAndDesignCouncil.Specifications
{
    #region Using Directives
    using System;
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
            List<ApplicationFormSection> mockFormSectionData = BuildMockFormSectionData();
            ApplicationForm emptyForm = new ApplicationForm {StartedOn=DateTime.Now};
            ApplicationForm partiallyCompletedForm = BuildPartiallyCompletedApplicationForm(mockFormSectionData);

            var mockFormSectionRepo = DependencyOf<INHibernateRepository<ApplicationFormSection>>();
            var mockFormRepo = DependencyOf<INHibernateRepository<ApplicationForm>>();
            mockFormSectionRepo.Stub(x => x.PerformQuery(null)).IgnoreArguments().Return(mockFormSectionData);
            mockFormRepo.Stub(x => x.Get(1)).Return(emptyForm);
            mockFormRepo.Stub(x => x.Get(2)).Return(partiallyCompletedForm);

        };
  
        private static ApplicationForm BuildPartiallyCompletedApplicationForm(List<ApplicationFormSection> formToComplete)
        {
            ApplicationForm aFrm = new ApplicationForm { StartedOn = DateTime.Now };

            QuestionAnswer answer1_1 = new QuestionAnswer { AnswerText = "Answer 1 1", Question = formToComplete[0].Questions[0] };
            QuestionAnswer answer1_2 = new QuestionAnswer { AnswerText = "Answer 1 2", Question = formToComplete[0].Questions[1] };
            QuestionAnswer answer2_1 = new QuestionAnswer { AnswerText = "Answer 2 1", Question = formToComplete[1].Questions[0] };
            aFrm.Answers = new List<QuestionAnswer>();
            aFrm.Answers.Add(answer1_1);
            aFrm.Answers.Add(answer1_2);
            aFrm.Answers.Add(answer2_1);
            return aFrm;
        }

        private static List<ApplicationFormSection> BuildMockFormSectionData()
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
            return mockFormSectionData;
        }
    }
}