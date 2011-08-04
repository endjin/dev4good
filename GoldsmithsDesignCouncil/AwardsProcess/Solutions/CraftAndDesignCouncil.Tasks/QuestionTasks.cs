namespace CraftAndDesignCouncil.Tasks
{
    using CraftAndDesignCouncil.Domain.Contracts.Tasks;
    using SharpArch.NHibernate.Contracts.Repositories;
    using CraftAndDesignCouncil.Domain;
    using CraftAndDesignCouncil.Domain.Contracts.Queries;


    public class QuestionTasks : IQuestionTasks
    {
        private INHibernateRepository<ApplicationFormSection> applicationFormSectionRepository;
        private IGetOrderedListOfSectionsQuery orderedListOfSectionsQuery;

        public QuestionTasks(INHibernateRepository<ApplicationFormSection> applicationFormSectionRepository
                                 , IGetOrderedListOfSectionsQuery orderedListOfSectionsQuery)
        {
            this.orderedListOfSectionsQuery = orderedListOfSectionsQuery;
            this.applicationFormSectionRepository = applicationFormSectionRepository;
        }

        public int GetNextRequiredSectionForApplicationForm(int applicationFormId)
        {
            var sections = applicationFormSectionRepository.PerformQuery(orderedListOfSectionsQuery);
            return sections[0].Id;
        }
    }
}