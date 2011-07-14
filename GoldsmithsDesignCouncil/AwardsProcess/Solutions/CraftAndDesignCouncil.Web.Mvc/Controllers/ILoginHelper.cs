namespace CraftAndDesignCouncil.Web.Mvc.Controllers
{
    using CraftAndDesignCouncil.Domain;

    public interface ILoginHelper
    {
        Applicant GetLoggedInApplicant();

        bool SomebodyIsLoggedIn { get; }

        void LoginApplicant(int applicantId);

        void LoginApplicant(string email, string password);

        void LogoutApplicant();
    }
}