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
            Brew brew = context.Brews.First();
            brew.Reviews = new List<Review>();
            Review review = new Review
            {
                ID = 1,
                Comments = "asd",
                HeadRating = 5,
                OverallRating = 4,
                TasteRating = 3
            };
            brew.Reviews.Add(review);
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }
    }
}