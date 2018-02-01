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
            string actors = "";
            foreach (Actor actor in movie.Actors)
            {
                if (String.IsNullOrEmpty(actors))
                {
                    actors = actor.Name;
                }
                else
                {
                    actors += ", " + actor.Name;
                }
            }

            ViewBag.actors = actors;

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
            db.Actors.Attach(a);
            m.Actors.Add(a);
            db.SaveChanges();
        }

        public void DeleteAllMovieActors(int movieId)
        {
            Movie m = db.Movies.Find(movieId);
            m.Actors.Clear();
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
        public ActionResult Create([Bind(Include = "Id,Name,Yor,PosterUrl,Plot")] Movie movie)
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
                    {
                        AddMovieActor(movie.Id, currentId);
                    }
                }
                foreach (string file in Request.Files)
                {
                    var postedFile = Request.Files[file];
                    if (!string.IsNullOrEmpty(postedFile.FileName))
                    {
                        postedFile.SaveAs(Server.MapPath("~/Images/") + movie.Id.ToString() + Path.GetFileName(postedFile.FileName));
                        movie.PosterFilePath = "~/Images/" + movie.Id.ToString() + Path.GetFileName(postedFile.FileName);
                    }
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
            List<int> actorsSelected = new List<int>();
            string[] actorIds = Request["actor"].Split(',');
            foreach (string actorId in actorIds)
            {
                int currentId = int.Parse(actorId);
                actorsSelected.Add(currentId);
            }
            if (ModelState.IsValid)
            {
                Movie m = db.Movies.Find(movie.Id);
                m.ProducerId = int.Parse(Request["producer"]);
                m.Actors.Clear();
                m.Name = movie.Name;
                m.Plot = movie.Plot;
                m.Producer = movie.Producer;
                foreach (string file in Request.Files)
                {
                    var postedFile = Request.Files[file];
                    if (!string.IsNullOrEmpty(postedFile.FileName))
                    {
                        string fullPath = Request.MapPath(m.PosterFilePath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        postedFile.SaveAs(Server.MapPath("~/Images/") + m.Id.ToString() + Path.GetFileName(postedFile.FileName));
                        m.PosterFilePath = "~/Images/" + m.Id.ToString() + Path.GetFileName(postedFile.FileName);
                    }
                }
                foreach (int actorId in actorsSelected)
                {
                    Actor a = db.Actors.Find(actorId);
                    m.Actors.Add(a);
                }
                db.Entry(m).State = EntityState.Modified;
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
