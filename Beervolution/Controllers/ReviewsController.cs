using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Beervolution.Models;

namespace Beervolution.Controllers
{
    public class ReviewsController : Controller
    {
        private BeerContext context = new BeerContext();

        // GET: Reviews
        public ActionResult Index()
        {
            return View(context.Reviews.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = context.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create/5
        public ActionResult Create(int brewID)
        {
            Review review = new Review(brewID);
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BrewID,TasteRating,HeadRating,ClarityRating,OverallRating,Comments")] Review review)
        {
            if (ModelState.IsValid)
            {
                int[] ratings = { review.TasteRating, review.HeadRating, review.ClarityRating };
                review.OverallRating = Convert.ToInt32(Math.Round(ratings.Average()));

                Brew brew = context.Brews.Include(b => b.Variables).FirstOrDefault(b => b.ID == review.BrewID);
                if (brew != null)
                {
                    string sid = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                    User currentUser = context.Users.FirstOrDefault(u => u.OID == sid);
                    review.CreatedBy = currentUser;
                    review.CreatedDate = DateTime.Now;

                    context.Reviews.Add(review);
                    brew.Reviews.Add(review);

                    context.SaveChanges();

                    return RedirectToAction("Details", "Beers", new { @id = brew.BeerID });
                }
            }
        
            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = context.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Review review)
        {
            if (ModelState.IsValid)
            {
                context.Entry(review).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = context.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = context.Reviews.Find(id);
            context.Reviews.Remove(review);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
