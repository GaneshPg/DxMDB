using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DxMDB.Repository;
using DxMDB.DAL;
using Newtonsoft.Json.Linq;

namespace DxMDB.Controllers
{
    public class ActorsController : Controller
    {
        private ActorsRepository actorsRepository = new ActorsRepository();

        // GET: Actors
        public ActionResult Index()
        {
            return View(actorsRepository.GetAllActors());
        }

        // GET: Actors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = actorsRepository.GetActorById(id ?? 1);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        public ActionResult ModalCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModalUpdateDB(string Name, string Gender, string DateOfBirth, string Bio)
        {
            Actor actor;
            try
            {
                actor = new Actor { Name = Name, Gender = Gender, DateOfBirth = DateTime.Parse(DateOfBirth), Bio = Bio };
            }
            catch (Exception e)
            {
                actor = new Actor { Name = Name, Gender = Gender, Bio = Bio };
            }

            actorsRepository.AddActor(actor);

            JObject jObject = JObject.FromObject(actor);
            System.Diagnostics.Debug.WriteLine("Actor id : " + actor.Id.ToString());

            return Content(jObject.ToString(), "application/json");

        }

        // GET: Actors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Gender,DateOfBirth,Bio")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                actorsRepository.AddActor(actor);
                TempData["Notification"] = actor.Name + " has been added succesfully to the actors database!";
                return RedirectToAction("Index");
            }

            return View(actor);
        }

        // GET: Actors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = actorsRepository.GetActorById(id ?? 1);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Gender,DateOfBirth,Bio")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                actorsRepository.UpdateActor(actor);
                TempData["Notification"] = actor.Name + " has been edited succesfully!";
                return RedirectToAction("Index");
            }
            return View(actor);
        }

        // GET: Actors/Delete/5
        public ActionResult Delete(int id)
        {
            Actor actor = actorsRepository.GetActorById(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Actor actor = actorsRepository.GetActorById(id);
            actorsRepository.DeleteActor(id);
            TempData["Notification"] = actor.Name + " has been deleted from the actors database!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                actorsRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
