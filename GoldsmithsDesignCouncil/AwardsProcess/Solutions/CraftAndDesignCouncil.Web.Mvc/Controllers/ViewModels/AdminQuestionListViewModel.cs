namespace CraftAndDesignCouncil.Web.Mvc.Controllers.ViewModels
{
    #region Using Directives
    using System.Collections.Generic;
    using CraftAndDesignCouncil.Domain;
    #endregion

    public class AdminQuestionListViewModel
    {
        public IList<Question> Questions { get; set; }
    }
}