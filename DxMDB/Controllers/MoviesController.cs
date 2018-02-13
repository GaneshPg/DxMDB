using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DxMDB.DAL;
using DxMDB.Repository;

namespace DxMDB.Controllers
{
    public class MoviesController : Controller
    {
        private MovieDBContext db = new MovieDBContext();
        private MoviesRepository moviesRepository = new MoviesRepository();
        private ActorsRepository actorsRepository = new ActorsRepository();
        private ProducersRepository producersRepository = new ProducersRepository();

        // GET: Movies
        public ActionResult Index(int? id)
        {

            int index = id ?? 1;
            var viewModel = new MoviesIndexViewModel();
            int entriesPerPage = viewModel.NumberOfRows * viewModel.NumberOfColumns;
            int skip = (index - 1) * entriesPerPage;

            viewModel.Movies = moviesRepository.GetMovies().OrderBy(movie => movie.Name).Skip(skip).Take(entriesPerPage).ToList();
            viewModel.PageNumber = index;
            viewModel.NumberOfPages = (int)Math.Ceiling((float)moviesRepository.GetMovieCount() / entriesPerPage);
            if (viewModel.NumberOfPages == 0)
                viewModel.NumberOfPages = 1;
            return View(viewModel);
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = moviesRepository.GetMovieById(id ?? 1);
            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            ViewBag.Producers = producersRepository.GetProducers();
            ViewBag.Actors = actorsRepository.GetActors();
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
                moviesRepository.AddMovie(movie, selectedActorsList);
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
            Movie movie = moviesRepository.GetMovieById(id ?? 1);
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
                Movie movieFromDB = moviesRepository.GetMovieById(movie.Id);
                movieFromDB.ProducerId = int.Parse(Request["producer"]);
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
                moviesRepository.UpdateMovie(movie, selectedActorsList);
                TempData["Notification"] = movie.Name + " has been edited succesfully!";
                return RedirectToAction("Index");
            }
            ViewBag.ProducerSelected = movie.ProducerId;
            ViewBag.ActorsSelected = selectedActorsList;
            ViewBag.Producers = producersRepository.GetProducers();
            ViewBag.Actors = actorsRepository.GetActors();
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = moviesRepository.GetMovieById(id ?? 1);
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
            Movie movie = moviesRepository.GetMovieById(id);
            if (!string.IsNullOrEmpty(movie.PosterFilePath) && movie.PosterFilePath != "~/Images/no-image.png")
            {
                string fullPath = Request.MapPath(movie.PosterFilePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            moviesRepository.DeleteMovie(id);
            TempData["Notification"] = movie.Name + " has been deleted from the movies database!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                moviesRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
