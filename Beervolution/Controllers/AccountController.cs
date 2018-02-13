using Microsoft.Owin.Security;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beervolution.Controllers
{
        public class AccountController : Controller
    {
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                // To execute a policy, you simply need to trigger an OWIN challenge.
                // You can indicate which policy to use by specifying the policy id as the AuthenticationType
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties() { RedirectUri = "/" }, Startup.SignInPolicyId);
            }
        }

        public void EditProfile()
        {
            if (Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties() { RedirectUri = "/" }, Startup.ProfilePolicyId);
            }
        }

        public void ResetPassword()
        {
            if (Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties() { RedirectUri = "/" }, Startup.PasswordPolicyId);
            }
        }

        public void SignOut()
        {
            // To sign out the user, you should issue an OpenIDConnect sign out request
            if (Request.IsAuthenticated)
            {
                IEnumerable<AuthenticationDescription> authTypes = HttpContext.GetOwinContext().Authentication.GetAuthenticationTypes();
                HttpContext.GetOwinContext().Authentication.SignOut(authTypes.Select(t => t.AuthenticationType).ToArray());
            }
        }
    }
}