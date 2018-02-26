using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Security.Claims;

using PagedList;

using Beervolution.Models;
using Beervolution.ViewModels;

namespace Beervolution.Controllers
{
    public class BeersController : Controller
    {
        private BeerContext context = new BeerContext();

        // GET: Beers
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // Set sorting parameters
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
            ViewBag.DateSortParam = sortOrder == "Date" ? "DateDesc" : "Date";
            ViewBag.PercentageSortParam = sortOrder == "Percentage" ? "PercentageDesc" : "Percentage";

            // Set search string from criteria
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // Query for beers
            var beers = from b in context.Beers
                           select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                beers = beers.Where(b => b.Name.Contains(searchString)
                                       || b.Manufacturer.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    beers = beers.OrderBy(b => b.Name);
                    break;
                case "NameDesc":
                    beers = beers.OrderByDescending(b => b.Name);
                    break;
                case "Percentage":
                    beers = beers.OrderBy(b => b.TargetPercentage);
                    break;
                case "PercentageDesc":
                    beers = beers.OrderByDescending(b => b.TargetPercentage);
                    break;
                default:
                    beers = beers.OrderBy(b => b.Name);
                    break;
            }

            int pageSize = Int32.Parse(ConfigurationManager.AppSettings["Beers Page Size"]);
            int pageNumber = (page ?? 1);

            return View(beers.ToPagedList(pageNumber, pageSize));
        }

        // GET: Beers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = context.Beers.Include(b => b.Brews.Select(br => br.Variables)).Include(b => b.Brews.Select(br => br.Reviews)).SingleOrDefault(b => b.ID == id);

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

                string sid = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                newBeer.Beer.CreatedDate = DateTime.Now;
                User currentUser = context.Users.First(u => u.OID == sid);
                currentUser.Beers.Add(newBeer.Beer);

                context.Beers.Add(newBeer.Beer);
                context.SaveChanges();

                return RedirectToAction("Details", "Beers", new { id = newBeer.Beer.ID });
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
            beer.Deleted = true;
            context.Entry(beer).State = EntityState.Modified;
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
