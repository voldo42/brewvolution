using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;

using Beervolution.Models;

namespace Beervolution.Controllers
{
    public class HomeController : Controller
    {
        BeerContext context = new BeerContext();

        public ActionResult Index()
        {
            string sid = "";
            string displayName = "";

            List<Claim> claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            if (claims.Count > 0)
            {
                sid = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                displayName = ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType).Value;
            }
            User currentUser = context.Users.FirstOrDefault(u => u.SID == sid);

            if (currentUser == null)
            {
                currentUser = new User
                {
                    SID = sid,
                    Name = displayName,
                    PermissionGroup = Models.User.Group.Reviewer,
                    CreatedDate = DateTime.Now
                };
                context.Users.Add(currentUser);
                context.SaveChanges();
            }
            else if (currentUser.Name != displayName)
            {
                currentUser.Name = displayName;
                context.SaveChanges();
            }


            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Error(string message)
        {
            ViewBag.Message = message;

            return View("Error");
        }

        [Authorize]
        public ActionResult Claims()
        {
            Claim displayName = ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType);
            ViewBag.DisplayName = displayName != null ? displayName.Value : string.Empty;
            return View();
        }
    }
}