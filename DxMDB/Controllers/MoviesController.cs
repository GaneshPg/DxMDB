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
        private MovieDBContext db = new MovieDBContext();

        // GET: Movies
        public ActionResult Index(int? id)
        {

            int index = id ?? 1;
            var viewModel = new MoviesIndexViewModel();
            int entriesPerPage = viewModel.NumberOfRows * viewModel.NumberOfColumns;
            int skip = (index - 1) * entriesPerPage;

            viewModel.Movies = db.Movies.Include(movie => movie.Producer).OrderBy(movie => movie.Name).Skip(skip).Take(entriesPerPage).ToList();
            viewModel.PageNumber = index;
            viewModel.NumberOfPages = (int)Math.Ceiling((float)db.Movies.Count() / entriesPerPage);
            return View(viewModel);
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

        public void AddNewMovieActor(Movie movie, int actorId)
        {
            db.Movies.Add(movie);
            Actor actor = new Actor { Id = actorId };
            db.Actors.Attach(actor);
            movie.Actors.Add(actor);
            db.SaveChanges();
        }

        public void AddMovieActor(int movieId, int actorId)
        {
            Movie movie = db.Movies.Find(movieId);
            Actor actor = db.Actors.Find(actorId);
            db.Actors.Attach(actor);
            movie.Actors.Add(actor);
            db.SaveChanges();
        }

        public void DeleteAllMovieActors(int movieId)
        {
            Movie movie = db.Movies.Find(movieId);
            movie.Actors.Clear();
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
        public ActionResult Create([Bind(Include = "Id,Name,YearOfRelease,PosterUrl,Plot")] Movie movie)
        {
            movie.ProducerId = int.Parse(Request["producer"]);
            string[] actorsIdString;
            List<int> selectedActorsList = new List<int>();
            if (!string.IsNullOrEmpty(Request["actor"]))
            {
                actorsIdString = Request["actor"].Split(',');
                foreach (string actorId in actorsIdString)
                {
                    selectedActorsList.Add(int.Parse(actorId));
                }
            }
            else
                actorsIdString = null;
            if (ModelState.IsValid)
            {
                foreach (int currentId in selectedActorsList)
                {
                    if (currentId == selectedActorsList.ElementAt(0))
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
                    else
                    {
                        movie.PosterFilePath = "~/Images/no-image.png";
                    }
                }
                db.SaveChanges();
                TempData["Notification"] = movie.Name + " has been added succesfully to the movies database!";
                return RedirectToAction("Index");
            }

            ViewBag.ProducerSelected = movie.ProducerId;
            ViewBag.ActorsSelected = selectedActorsList;
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
            List<int> selectedActorsList = new List<int>();
            foreach (Actor actor in movie.Actors)
                selectedActorsList.Add(actor.Id);

            ViewBag.ProducerSelected = movie.ProducerId;
            ViewBag.ActorsSelected = selectedActorsList;
            ViewBag.Producers = db.Producers.ToList();
            ViewBag.Actors = db.Actors.ToList();
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,YearOfRelease,Plot,ProducerId")] Movie movie)
        {
            List<int> selectedActorsList = new List<int>();
            string[] actorsIdString = Request["actor"].Split(',');
            foreach (string actorId in actorsIdString)
            {
                int currentId = int.Parse(actorId);
                selectedActorsList.Add(currentId);
            }
            if (ModelState.IsValid)
            {
                Movie movieFromDB = db.Movies.Find(movie.Id);
                movieFromDB.ProducerId = int.Parse(Request["producer"]);
                movieFromDB.Actors.Clear();
                movieFromDB.Name = movie.Name;
                movieFromDB.Plot = movie.Plot;
                movieFromDB.Producer = movie.Producer;
                movieFromDB.YearOfRelease = movie.YearOfRelease;
                foreach (string file in Request.Files)
                {
                    var postedFile = Request.Files[file];
                    if (!string.IsNullOrEmpty(postedFile.FileName))
                    {
                        if (movieFromDB.PosterFilePath != "~/Images/no-image.png")
                        {
                            string fullPath = Request.MapPath(movieFromDB.PosterFilePath);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                        postedFile.SaveAs(Server.MapPath("~/Images/") + movieFromDB.Id.ToString() + Path.GetFileName(postedFile.FileName));
                        movieFromDB.PosterFilePath = "~/Images/" + movieFromDB.Id.ToString() + Path.GetFileName(postedFile.FileName);
                    }
                }
                foreach (int actorId in selectedActorsList)
                {
                    Actor actor = db.Actors.Find(actorId);
                    movieFromDB.Actors.Add(actor);
                }
                db.Entry(movieFromDB).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Notification"] = movie.Name + " has been edited succesfully!";
                return RedirectToAction("Index");
            }
            ViewBag.ProducerSelected = movie.ProducerId;
            ViewBag.ActorsSelected = selectedActorsList;
            ViewBag.Producers = db.Producers.ToList();
            ViewBag.Actors = db.Actors.ToList();
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
            if (!string.IsNullOrEmpty(movie.PosterFilePath) && movie.PosterFilePath != "~/Images/no-image.png")
            {
                string fullPath = Request.MapPath(movie.PosterFilePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            db.Movies.Remove(movie);
            db.SaveChanges();
            TempData["Notification"] = movie.Name + " has been deleted from the movies database!";
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
