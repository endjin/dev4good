namespace CraftAndDesignCouncil.Tasks
{
    using System;
    using System.Linq;
    using CraftAndDesignCouncil.Domain.Contracts.Tasks;
    using SharpArch.NHibernate.Contracts.Repositories;
    using CraftAndDesignCouncil.Domain;
    using CraftAndDesignCouncil.Domain.Contracts.Queries;

    public class ApplicationFormSectionTasks : IApplicationFormSectionTasks
    {
        private INHibernateRepository<ApplicationFormSection> applicationFormSectionRepository;
        private INHibernateRepository<ApplicationForm> applicationFormRepository;
        private IGetOrderedListOfSectionsQuery orderedListOfSectionsQuery;

        public ApplicationFormSectionTasks(INHibernateRepository<ApplicationFormSection> applicationFormSectionRepository
                                 , INHibernateRepository<ApplicationForm> applicationFormRepository
                                 , IGetOrderedListOfSectionsQuery orderedListOfSectionsQuery)
        {
            this.orderedListOfSectionsQuery = orderedListOfSectionsQuery;
            this.applicationFormSectionRepository = applicationFormSectionRepository;
            this.applicationFormRepository = applicationFormRepository;
        }

        public ApplicationFormSection Get(int id)
        {
            return applicationFormSectionRepository.Get(id);
        }

        public ApplicationFormSection GetNextRequiredSectionForApplicationForm(int applicationFormId)
        {
            var sections = applicationFormSectionRepository.PerformQuery(orderedListOfSectionsQuery);
            ApplicationFormSection nextRequiredSection = null;
            ApplicationForm form = applicationFormRepository.Get(applicationFormId);
            foreach (ApplicationFormSection section in sections)
            {
                if (SectionIsRequired(section, form)
                    && !SectionIsCompleted(section, form))
                {
                    nextRequiredSection = section;
                    break;
                }
            }

            return nextRequiredSection;
        }
  
        private bool SectionIsRequired(ApplicationFormSection section, ApplicationForm applicationForm)
        {
            if (section.NotRequiredIfQuestion == null) return true;

            return !applicationForm.Answers.Any(x => x.Question.Id == section.NotRequiredIfQuestion.Id
                                                        && x.AnswerText == section.NotRequiredIfAnswer);

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