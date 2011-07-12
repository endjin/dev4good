namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    using System.Web;

    public class MockLoginHelper : ILoginHelper
    { 
        private const string SESSION_KEY_LOGGED_IN_USER = "LOGGED_IN_USER";

        public bool SomebodyIsLoggedIn
        {
            get
            {
                return HttpContext.Current.Session[SESSION_KEY_LOGGED_IN_USER] != null;          
            }
        }

        public void LoginApplicant(int applicantId)
        {
            HttpContext.Current.Session["LOGGED_IN_USER"] = applicantId;
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