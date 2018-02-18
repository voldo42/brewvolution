﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Beervolution.Models;
using System.Threading;
using System.Globalization;
using System.Security.Claims;

using Beervolution.ViewModels;

namespace Beervolution.Controllers
{
    public class BrewsController : Controller
    {
        private BeerContext context = new BeerContext();

        // GET: Brews
        public ActionResult Index()
        {
            List<Beer> beers = context.Beers.Include(b => b.Brews.Select(s => s.Variables)).ToList();
            return View(beers);
        }

        // GET: Brews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brew brew = context.Brews.Find(id);
            if (brew == null)
            {
                return HttpNotFound();
            }
            return View(brew);
        }

        // GET: Brews/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // GET: Brews/Create/5
        public ActionResult Create(int beerID)
        {
            CreateBrewViewModel viewModel = new CreateBrewViewModel
            {
                BeerID = beerID
            };

            return View(viewModel);
        }

        // POST: Brews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BeerID,Brew,NewWaterType,NewFermentableType,Variables")] CreateBrewViewModel newBrew)
        {
            if (ModelState.IsValid)
            {
                newBrew.Brew.Variables.WaterType =
                    newBrew.Brew.Variables.WaterType == "Create New" ? newBrew.NewWaterType : newBrew.Brew.Variables.WaterType;

                newBrew.Brew.Variables.FermentableType =
                    newBrew.Brew.Variables.FermentableType == "Create New" ? newBrew.NewFermentableType : newBrew.Brew.Variables.FermentableType;

                string sid = ClaimsPrincipal.Current.Identities.First().Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                User currentUser = context.Users.FirstOrDefault(u => u.SID == sid);
                currentUser.Brews.Add(newBrew.Brew);

                Beer beer = context.Beers.Find(newBrew.BeerID);
                beer.Brews.Add(newBrew.Brew);

                context.SaveChanges();
                return RedirectToAction("Details", "Beers", new { id = beer.ID });
            }

            return View(newBrew);
        }

        // GET: Brews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brew brew = context.Brews.Include(b => b.Variables).SingleOrDefault(b => b.ID == id);
            if (brew == null)
            {
                return HttpNotFound();
            }

            CreateBrewViewModel viewModel = new CreateBrewViewModel
            {
                Brew = brew,
                BeerID = brew.BeerID,
                OriginalWaterType = brew.Variables.WaterType,
                OriginalFermentableType = brew.Variables.FermentableType
            };

            return View(viewModel);
        }

        // POST: Brews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,StartDate,BottleDate,StartingGravity,FinalGravity,SecondaryFermentationDate,Variables")] Brew brew)
        {
            if (ModelState.IsValid)
            {
                context.Entry(brew).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brew);
        }

        // GET: Brews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brew brew = context.Brews.Find(id);
            if (brew == null)
            {
                return HttpNotFound();
            }
            return View(brew);
        }

        // POST: Brews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brew brew = context.Brews.Find(id);
            context.Brews.Remove(brew);
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

        public JsonResult GetWaterTypes(string selectedItem)
        {
            List<SelectListItem> waterTypeList = new List<SelectListItem>();
            var waterTypes = context.Variables.Select(v => v.WaterType).Distinct().ToList();

            waterTypes.OrderBy(w => w);
            waterTypes.Remove("");
            waterTypes.Remove(null);
            waterTypes.Add("");
            waterTypes.Add("Create New");

            waterTypes.ForEach(w => waterTypeList.Add(new SelectListItem { Text = w, Value = w }));
            waterTypeList.Single(w => w.Value == selectedItem).Selected = true;

            return Json(new SelectList(waterTypeList, "Value", "Text"));
        }

        public JsonResult GetFermentableTypes(string selectedItem)
        {
            List<SelectListItem> fermentableTypeList = new List<SelectListItem>();
            var fermentableTypes = context.Variables.Select(v => v.FermentableType).Distinct().ToList();

            fermentableTypes.OrderBy(f => f);
            fermentableTypes.Remove("");
            fermentableTypes.Remove(null);
            fermentableTypes.Add("");
            fermentableTypes.Add("Create New");

            fermentableTypes.ForEach(f => fermentableTypeList.Add(new SelectListItem { Text = f, Value = f }));
            fermentableTypeList.Single(f => f.Value == selectedItem).Selected = true;

            return Json(new SelectList(fermentableTypeList, "Value", "Text"));
        }
    }
}
