using Beervolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beervolution.Controllers
{
    public class HomeController : Controller
    {
        BeerContext context = new BeerContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }
    }
}