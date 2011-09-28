using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CraftAndDesignCouncil.Domain.Contracts.Tasks
{
    public interface IApplicationFormTasks
    {
        ApplicationForm StartNewApplicationForm(Applicant applicant);
        ApplicationForm Get(int id);
    }
}
