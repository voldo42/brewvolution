using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Beervolution.Models;

namespace Beervolution.Controllers
{
    public class BeersController : Controller
    {
        private BeerContext context = new BeerContext();

        // GET: Beers
        public ActionResult Index()
        {
            return View(context.Beers.ToList());
        }

        // GET: Beers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = context.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // GET: Beers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Manufacturer,Type,InclusiveKit,TargetPercentage")] Beer beer)
        {
            if (ModelState.IsValid)
            {
                beer.Brews = new List<Brew>();
                context.Beers.Add(beer);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(beer);
        }

        // GET: Beers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = context.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Beers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Manufacturer,Type,InclusiveKit,TargetPercentage")] Beer beer)
        {
            if (ModelState.IsValid)
            {
                context.Entry(beer).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beer);
        }

        // GET: Beers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = context.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Beers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Beer beer = context.Beers.Find(id);
            context.Beers.Remove(beer);
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

        public JsonResult GetBeers(int? id)
        {
            List<SelectListItem> beerList = new List<SelectListItem>();

            var beers = context.Beers.ToList();
            beers.OrderBy(b => b);
            beers.ForEach(b => beerList.Add(new SelectListItem { Text = String.Format("{0} ({1})", b.Name, b.Manufacturer), Value = b.ID.ToString() }));

            return Json(new SelectList(beerList, "Value", "Text"));
        }
    }
}
