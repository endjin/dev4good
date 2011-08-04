namespace CraftAndDesignCouncil.Tasks
{
    using System;
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

        public ApplicationFormSection GetNextRequiredSectionForApplicationForm(int applicationFormId)
        {
            var sections = applicationFormSectionRepository.PerformQuery(orderedListOfSectionsQuery);
            ApplicationFormSection nextRequiredSection = null;
            foreach (ApplicationFormSection section in sections)
            {
                if (SectionIsRequired(section, applicationFormId)
                     && !SectionIsCompleted(section, applicationFormId))
                {
                    nextRequiredSection = section;
                    break;
                }
            }

            return nextRequiredSection;
        }
  
        private bool SectionIsRequired(ApplicationFormSection section, int applicationFormId)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
  
        private bool SectionIsCompleted(ApplicationFormSection section, int applicationFormId)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}