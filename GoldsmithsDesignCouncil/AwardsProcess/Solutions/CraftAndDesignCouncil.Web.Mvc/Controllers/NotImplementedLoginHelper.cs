using System;

namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    public class NotImplementedLoginHelper : ILoginHelper
    {
        public bool SomebodyIsLoggedIn
        {
            get
            {
                // TODO: Implement this property getter
                throw new NotImplementedException();
            }
        }

        public void LoginApplicant(int applicantId)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void LoginApplicant(string email, string password)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void LogoutApplicant()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}