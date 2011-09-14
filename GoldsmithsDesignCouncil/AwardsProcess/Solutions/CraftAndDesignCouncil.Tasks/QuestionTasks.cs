namespace CraftAndDesignCouncil.Tasks
{
    using System;
    using System.Linq;
    using CraftAndDesignCouncil.Domain.Contracts.Tasks;
    using SharpArch.NHibernate.Contracts.Repositories;
    using CraftAndDesignCouncil.Domain;
    using CraftAndDesignCouncil.Domain.Contracts.Queries;

    public class QuestionTasks : IQuestionTasks
    {
        private INHibernateRepository<ApplicationFormSection> applicationFormSectionRepository;
        private INHibernateRepository<ApplicationForm> applicationFormRepository;
        private IGetOrderedListOfSectionsQuery orderedListOfSectionsQuery;

        public QuestionTasks(INHibernateRepository<ApplicationFormSection> applicationFormSectionRepository
                                 , INHibernateRepository<ApplicationForm> applicationFormRepository
                                 , IGetOrderedListOfSectionsQuery orderedListOfSectionsQuery)
        {
            this.orderedListOfSectionsQuery = orderedListOfSectionsQuery;
            this.applicationFormSectionRepository = applicationFormSectionRepository;
            this.applicationFormRepository = applicationFormRepository;
        }

        public ApplicationFormSection GetNextRequiredSectionForApplicationForm(int applicationFormId)
        {
            var sections = applicationFormSectionRepository.PerformQuery(orderedListOfSectionsQuery);
            ApplicationFormSection nextRequiredSection = null;
            ApplicationForm form = applicationFormRepository.Get(applicationFormId);
            foreach (ApplicationFormSection section in sections)
            {
                //if (SectionIsRequired(section, applicationFormId)
                if (!SectionIsCompleted(section, form))
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
  
        private bool SectionIsCompleted(ApplicationFormSection section, ApplicationForm applicationForm)
        {

            if (section == null) return true;
            if (section.Questions == null || section.Questions.Count <1) return true;
            if (applicationForm == null) return false;
            if (section.Questions.Count>0 && applicationForm.Answers == null) return false;

            return section.Questions.All(question => 
                            applicationForm.Answers.Any(answer => 
                                answer.Question.Id == question.Id));        
        }
    }
}