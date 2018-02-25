using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
            string oid = "";
            string displayName = "";
            string newOID = "";

            List<Claim> claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();

            if (claims.Count > 0)
            {
                oid = claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                displayName = ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType).Value;
                newOID = ((ClaimsIdentity)User.Identity).FindFirst("objectId").Value;

                User currentUser = context.Users.Include(b => b.Brews).FirstOrDefault(u => u.OID == oid);

                if (currentUser == null)
                {
                    currentUser = new User
                    {
                        OID = oid,
                        Name = displayName,
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

                ViewBag.NoOfBrews = currentUser.Brews.Count();
                ViewBag.NoOfComments = 0;
                ViewBag.MemberSince = currentUser.CreatedDate.ToShortDateString();
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