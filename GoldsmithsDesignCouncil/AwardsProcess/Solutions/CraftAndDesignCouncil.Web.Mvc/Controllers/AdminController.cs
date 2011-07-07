
namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    #region Using Directives
    using CraftAndDesignCouncil.Domain.Contracts.Queries;
    using System.Web.Mvc;
    using CraftAndDesignCouncil.Web.Mvc.Controllers.ViewModels;
    #endregion

    public class AdminController : Controller
    {
        private readonly IAllQuestionsQuery allQuestionsQuery;

        public AdminController(IAllQuestionsQuery allQuestionsQuery)
        {
            this.allQuestionsQuery = allQuestionsQuery;
        }

        public ActionResult Index()
        {
            var viewModel = new AdminQuestionListViewModel() {Questions = allQuestionsQuery.ExecuteQuery()};
            return View(viewModel);
        }
    }
}
