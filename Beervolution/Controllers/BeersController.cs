using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Beervolution.Models;
using Beervolution.ViewModels;

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
        public ActionResult Create([Bind(Include = "Beer,NewManufacturer,NewType")] CreateBeerViewModel newBeer)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(newBeer.NewManufacturer))
                {
                    Manufacturer manufacturer = new Manufacturer
                    {
                        Name = newBeer.NewManufacturer
                    };

                    context.Manufacturers.Add(manufacturer);
                    context.SaveChanges();

                    newBeer.Beer.Manufacturer = manufacturer;
                }
                else
                {
                    Manufacturer manufacturer = context.Manufacturers.Find(newBeer.Beer.ManufacturerID);
                    newBeer.Beer.Manufacturer = manufacturer;
                }

                newBeer.Beer.Type = newBeer.Beer.Type == "Create New" ? newBeer.NewType : newBeer.Beer.Type;

                context.Beers.Add(newBeer.Beer);
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(newBeer);
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

            CreateBeerViewModel viewModel = new CreateBeerViewModel
            {
                Beer = beer,
                OriginalManufacturer = beer.Manufacturer.ID,
                OriginalBeerType = beer.Type
            };

            return View(viewModel);
        }

        // POST: Beers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Beer,NewManufacturer,OriginalManufacturer,NewType,OriginalBeerType")] CreateBeerViewModel editBeer)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(editBeer.NewManufacturer))
                {
                    Manufacturer manufacturer = new Manufacturer
                    {
                        Name = editBeer.NewManufacturer
                    };

                    context.Manufacturers.Add(manufacturer);
                    context.SaveChanges();

                    editBeer.Beer.Manufacturer = manufacturer;
                    editBeer.Beer.ManufacturerID = manufacturer.ID;
                }
                else
                {
                    Manufacturer manufacturer = context.Manufacturers.Find(editBeer.Beer.ManufacturerID);
                    editBeer.Beer.Manufacturer = manufacturer;
                    editBeer.Beer.ManufacturerID = manufacturer.ID;
                }

                editBeer.Beer.Type = editBeer.Beer.Type == "Create New" ? editBeer.NewType : editBeer.Beer.Type;

                context.Entry(editBeer.Beer).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(editBeer);
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

        public JsonResult GetBeers()
        {
            List<SelectListItem> beerList = new List<SelectListItem>();

            var beers = context.Beers.ToList();
            beers.OrderBy(b => b);
            beers.ForEach(b => beerList.Add(new SelectListItem { Text = String.Format("{0} ({1})", b.Name, b.Manufacturer.Name), Value = b.ID.ToString() }));

            return Json(new SelectList(beerList, "Value", "Text"));
        }

        public JsonResult GetManufacturers(string selectedItem)
        {
            List<SelectListItem> manufacturerList = new List<SelectListItem>();
            var manufacturers = context.Manufacturers.ToList();

            manufacturers.OrderBy(m => m);
            manufacturers.Remove(null);

            manufacturers.ForEach(m => manufacturerList.Add(new SelectListItem { Text = m.Name, Value = m.ID.ToString() }));

            if (!String.IsNullOrEmpty(selectedItem))
            {
                manufacturerList.Single(m => m.Value == selectedItem).Selected = true;
            }

            manufacturerList.Add(new SelectListItem { Text = "", Value = "" });
            manufacturerList.Add(new SelectListItem { Text = "Create New", Value = "0" });

            return Json(new SelectList(manufacturerList, "Value", "Text"));
        }

        public JsonResult GetBeerTypes(string selectedItem)
        {
            List<SelectListItem> beerTypeList = new List<SelectListItem>();
            var beerTypes = context.Beers.Select(b => b.Type).Distinct().ToList();

            beerTypes.OrderBy(b => b);
            beerTypes.Remove("");
            beerTypes.Remove(null);
            beerTypes.Add("");
            beerTypes.Add("Create New");

            beerTypes.ForEach(b => beerTypeList.Add(new SelectListItem { Text = b, Value = b }));

            if (!String.IsNullOrEmpty(selectedItem))
            {
                beerTypeList.Single(b => b.Value == selectedItem).Selected = true;
            }

            return Json(new SelectList(beerTypeList, "Value", "Text"));
        }
    }
}
