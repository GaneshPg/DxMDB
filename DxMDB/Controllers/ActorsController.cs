using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DxMDB.Models;
using Newtonsoft.Json.Linq;

namespace DxMDB.Controllers
{
    public class ActorsController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        static string RenderViewToString(ControllerContext context,
                                    string viewPath,
                                    object model = null,
                                    bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }

        // GET: Actors
        public ActionResult Index()
        {
            return View(db.Actors.ToList());
        }

        // GET: Actors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
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
        public ActionResult ModalUpdateDB(string Name, string Gender, string DOB, string Bio)
        {
            Actor actor;
            try
            {
                actor = new Actor { Name = Name, Gender = Gender, DOB = DateTime.Parse(DOB), Bio = Bio };
            }
            catch (Exception e)
            {
                actor = new Actor { Name = Name, Gender = Gender, Bio = Bio };
            }
            JObject jp = JObject.FromObject(actor);
            if (TryValidateModel(actor))
            {
                db.Actors.Add(actor);
                db.SaveChanges();
                jp.Add("error", "false");
                return Content(jp.ToString(), "application/json");
            }

            jp.Add("error", "true");
            var view = RenderViewToString(this.ControllerContext, "~/Views/Actors/ModalCreate.cshtml", actor);

            jp.Add("view", view);
            return Content(jp.ToString(), "application/json");

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
        public ActionResult Create([Bind(Include = "Id,Name,Gender,DOB,Bio")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                db.Actors.Add(actor);
                db.SaveChanges();
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
            Actor actor = db.Actors.Find(id);
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
        public ActionResult Edit([Bind(Include = "Id,Name,Gender,DOB,Bio")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actor).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Notification"] = actor.Name + " has been edited succesfully!";
                return RedirectToAction("Index");
            }
            return View(actor);
        }

        // GET: Actors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
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
            Actor actor = db.Actors.Find(id);
            db.Actors.Remove(actor);
            db.SaveChanges();
            TempData["Notification"] = actor.Name + " has been deleted from the actors database!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
