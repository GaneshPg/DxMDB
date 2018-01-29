using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DxMDB.Models;

namespace DxMDB.Controllers
{
    public class MoviesController : Controller
    {
        public MovieDBContext db = new MovieDBContext();

        // GET: Movies
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                return RedirectToAction("Details", new { id = id });
            }
            var movies = db.Movies.Include(m => m.Producer);
            return View(movies.ToList());
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        public void AddNewMovieActor(Movie m, int actorId)
        {
            db.Movies.Add(m);
            Actor a = new Actor { Id = actorId };
            db.Actors.Attach(a);
            m.Actors.Add(a);
            db.SaveChanges();
        }

        public void AddMovieActor(int movieId, int actorId)
        {
            Movie m = db.Movies.Find(movieId);
            Actor a = db.Actors.Find(actorId);
            db.Movies.Attach(m);
            db.Actors.Attach(a);
            m.Actors.Add(a);
            db.SaveChanges();
        }

        public void DeleteAllMovieActors(int movieId)
        {
            Movie m = db.Movies.Find(movieId);

            foreach (Actor a in m.Actors)
                m.Actors.Remove(a);

            db.SaveChanges();
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            ViewBag.Producers = db.Producers;
            ViewBag.Actors = db.Actors;
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Yor,Plot")] Movie movie)
        {
            movie.ProducerId = int.Parse(Request["producer"]);
            List<int> actorsSelected = new List<int>();
            string[] actorIds = Request["actor"].Split(',');
            foreach (string actorId in actorIds)
            {
                actorsSelected.Add(int.Parse(actorId));
            }
                if (ModelState.IsValid)
            {
                foreach (int currentId in actorsSelected)
                {
                    if (currentId == actorsSelected.ElementAt(0))
                        AddNewMovieActor(movie, currentId);
                    else
                        AddMovieActor(movie.Id, currentId);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProducerSelected = movie.ProducerId;
            ViewBag.ActorsSelected = actorsSelected;
            ViewBag.Producers = db.Producers;
            ViewBag.Actors = db.Actors;
            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            List<int> actorsSelected = new List<int>();
            foreach (Actor a in movie.Actors)
                actorsSelected.Add(a.Id);
            ViewBag.ProducerSelected = movie.ProducerId;
            ViewBag.ActorsSelected = actorsSelected;
            ViewBag.Producers = db.Producers;
            ViewBag.Actors = db.Actors;
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Yor,Plot,ProducerId")] Movie movie)
        {
            movie.ProducerId = int.Parse(Request["producer"]);
            List<int> actorsSelected = new List<int>();
            string[] actorIds = Request["actor"].Split(',');
            foreach (string actorId in actorIds)
            {
                int currentId = int.Parse(actorId);
                actorsSelected.Add(currentId);
            }
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                DeleteAllMovieActors(movie.Id);
                foreach (int currentId in actorsSelected)
                    AddMovieActor(movie.Id, currentId);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProducerSelected = movie.ProducerId;
            ViewBag.ActorsSelected = actorsSelected;
            ViewBag.Producers = db.Producers;
            ViewBag.Actors = db.Actors;
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
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
