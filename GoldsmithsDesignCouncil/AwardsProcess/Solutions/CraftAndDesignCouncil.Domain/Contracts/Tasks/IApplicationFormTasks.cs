using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CraftAndDesignCouncil.Domain.Contracts.Tasks
{
    public interface IApplicationFormTasks
    {
        ApplicationForm Get(int id);
    }
}
