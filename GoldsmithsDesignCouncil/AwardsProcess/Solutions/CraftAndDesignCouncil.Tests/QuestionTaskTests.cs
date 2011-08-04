namespace CraftAndDesignCouncil.Tests
{
    using NUnit.Framework;
    using CraftAndDesignCouncil.Tasks;
    using Microsoft.Practices.ServiceLocation;
    using SharpArch.NHibernate.Contracts.Repositories;
    using CraftAndDesignCouncil.Domain;

    [TestFixture]
    public class QuestionTaskTests
    {
        [TestCase]
        public void NextRequiredSectionForNewApplicationFormIsFirstSection()
        {
            ServiceLocatorInitializer.Init();
            INHibernateRepository<ApplicationFormSection> repo = ServiceLocator.Current.GetInstance<INHibernateRepository<ApplicationFormSection>>();
            QuestionTasks OUT = new QuestionTasks(repo, null);
            int result = OUT.GetNextRequiredSectionForApplicationForm(2);
            Assert.AreEqual(10, result);
        }
    }
}