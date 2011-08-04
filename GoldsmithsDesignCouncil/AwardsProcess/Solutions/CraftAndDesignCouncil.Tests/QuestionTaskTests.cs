namespace CraftAndDesignCouncil.Tests
{
    #region Using Directives
    using CraftAndDesignCouncil.Domain;
    using CraftAndDesignCouncil.Tasks;
    using Microsoft.Practices.ServiceLocation;
    using NUnit.Framework;
    using SharpArch.NHibernate.Contracts.Repositories;
    using Rhino.Mocks;
    using System.Collections.Generic;
    #endregion

    [TestFixture]
    public class QuestionTaskTests
    {
        [SetUp]
        public void Setup()
        {
            ServiceLocatorInitializer.Init();

            List<ApplicationFormSection> mockFormSectionData = new List<ApplicationFormSection>();
            for (int x = 0; x < 10; x++)
            {
                mockFormSectionData.Add(new ApplicationFormSection { Title = "Section " + x });
            }
            var mockFormSectionRepo = ServiceLocator.Current.GetInstance<INHibernateRepository<ApplicationFormSection>>();

            mockFormSectionRepo.Stub(x => x.PerformQuery(null)).IgnoreArguments().Return(mockFormSectionData);
        }

        [TestCase]
        public void NextRequiredSectionForNewApplicationFormIsFirstSection()
        {
            INHibernateRepository<ApplicationFormSection> repo = ServiceLocator.Current.GetInstance<INHibernateRepository<ApplicationFormSection>>();
            QuestionTasks OUT = new QuestionTasks(repo, null);
            ApplicationFormSection result = OUT.GetNextRequiredSectionForApplicationForm(2);
            Assert.AreEqual("Section 1", result.Title);
        }

        //CompletedSectionsAreSkippedWhenLookingForNextSection
        //NonRequiredSectionsAreTakenIntoAccoutWhenLookingForNextSection

        //CanTellIfASectionIsRequired
        //CanTellIfASectionIsComplete
        //PartCompletedSectionsAreNotComplete
  
       
    }
}