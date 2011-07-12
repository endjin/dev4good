using System;

namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    public interface ILoginHelper
    {
        bool SomebodyIsLoggedIn { get; }

        void LoginApplicant(int applicantId);

        void LoginApplicant(string email, string password);

        void LogoutApplicant();
    }
}