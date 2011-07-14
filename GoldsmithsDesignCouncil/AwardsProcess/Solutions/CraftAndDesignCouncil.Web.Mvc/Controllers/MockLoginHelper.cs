namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    #region Using Directives
    using System.Web;
    using CraftAndDesignCouncil.Domain;
    using CraftAndDesignCouncil.Domain.Contracts.Tasks;
    #endregion

    public class MockLoginHelper : ILoginHelper
    { 
        private const string SESSION_KEY_LOGGED_IN_USER = "LOGGED_IN_USER";

        private readonly IApplicantTasks applicantTasks;

        public MockLoginHelper(IApplicantTasks applicantTasks)
        {
            this.applicantTasks = applicantTasks;
        }

        public Applicant GetLoggedInApplicant()
        {
            Applicant result = applicantTasks.Get((int)HttpContext.Current.Session[SESSION_KEY_LOGGED_IN_USER]);
            return result;
        }

        public bool SomebodyIsLoggedIn
        {
            get
            {
                return HttpContext.Current.Session[SESSION_KEY_LOGGED_IN_USER] != null;          
            }
        }

        public void LoginApplicant(int applicantId)
        {
            HttpContext.Current.Session[SESSION_KEY_LOGGED_IN_USER] = applicantId;
        }

        public void LoginApplicant(string email, string password)
        {
            //TODO : Just mocking this up for now need to write a task to actually do the logging in
            HttpContext.Current.Session[SESSION_KEY_LOGGED_IN_USER] = 1;
        }

        public void LogoutApplicant()
        {
            HttpContext.Current.Session.Remove(SESSION_KEY_LOGGED_IN_USER);
        }
    }
}