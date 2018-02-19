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
        private MoviesRepository moviesRepository = new MoviesRepository();
        private ActorsRepository actorsRepository = new ActorsRepository();
        private ProducersRepository producersRepository = new ProducersRepository();

        // GET: Movies
        public ActionResult Index(int? id, string key)
        {

            int index = id ?? 1;
            var viewModel = new MoviesIndexViewModel();
            int entriesPerPage = viewModel.NumberOfRows * viewModel.NumberOfColumns;
            int skip = (index - 1) * entriesPerPage;
            int count;

            if (string.IsNullOrEmpty(key))
            {
                viewModel.Movies = moviesRepository.GetAllMovies().OrderBy(movie => movie.Name).Skip(skip).Take(entriesPerPage).ToList();
                count = moviesRepository.GetCount();
            }
            else
            {
                IEnumerable<Movie> selectedMoviesList = moviesRepository.GetAllMovies().Where(movie => movie.Name.ToUpper().Contains(key.ToUpper()));
                count = selectedMoviesList.Count();
                viewModel.Movies = selectedMoviesList.OrderBy(movie => movie.Name).Skip(skip).Take(entriesPerPage).ToList();
            }
            viewModel.PageNumber = index;
            viewModel.NumberOfPages = (int)Math.Ceiling((float)count / entriesPerPage);
            if (viewModel.NumberOfPages == 0)
                viewModel.NumberOfPages = 1;
            viewModel.key = key;
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
            ViewBag.Producers = producersRepository.GetAllProducers();
            ViewBag.Actors = actorsRepository.GetAllActors();
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
                //foreach (int currentId in selectedActorsList)
                //{
                //    if (currentId == selectedActorsList.ElementAt(0))
                //        AddNewMovieActor(movie, currentId);
                //    else
                //    {
                //        AddMovieActor(movie.Id, currentId);
                //    }
                //}
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
            ViewBag.Producers = producersRepository.GetAllProducers();
            ViewBag.Actors = actorsRepository.GetAllActors();
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
            ViewBag.Producers = producersRepository.GetAllProducers();
            ViewBag.Actors = actorsRepository.GetAllActors();
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,YearOfRelease,Plot")] Movie movie)
        {
            List<int> selectedActorsList = new List<int>();
            string[] actorsIdString = Request["actor"].Split(',');
            foreach (string actorId in actorsIdString)
            {
                int currentId = int.Parse(actorId);
                selectedActorsList.Add(currentId);
            }
            try
            {
                movie.ProducerId = int.Parse(Request["producer"]);
            }
            catch(Exception e)
            {
                movie.ProducerId = -1;
            }
            if (ModelState.IsValid)
            {
                Movie movieFromDB = moviesRepository.GetMovieById(movie.Id);
                //movieFromDB.ProducerId = int.Parse(Request["producer"]);
                //movieFromDB.Actors.Clear();
                //movieFromDB.Name = movie.Name;
                //movieFromDB.Plot = movie.Plot;
                //movieFromDB.Producer = movie.Producer;
                //movieFromDB.YearOfRelease = movie.YearOfRelease;
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
                        movie.PosterFilePath = "~/Images/" + movieFromDB.Id.ToString() + Path.GetFileName(postedFile.FileName);
                    }
                    else
                    {
                        movie.PosterFilePath = movieFromDB.PosterFilePath;
                    }
                }
                //foreach (int actorId in selectedActorsList)
                //{
                //    Actor actor = db.Actors.Find(actorId);
                //    movieFromDB.Actors.Add(actor);
                //}
                //db.Entry(movieFromDB).State = EntityState.Modified;
                //db.SaveChanges();
                moviesRepository.UpdateMovie(movie, selectedActorsList, movieFromDB);
                TempData["Notification"] = movie.Name + " has been edited succesfully!";
                return RedirectToAction("Index");
            }
            ViewBag.ProducerSelected = movie.ProducerId;
            ViewBag.ActorsSelected = selectedActorsList;
            ViewBag.Producers = producersRepository.GetAllProducers();
            ViewBag.Actors = actorsRepository.GetAllActors();
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int id)
        {
            Movie movie =moviesRepository.GetMovieById(id);
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
            TempData["Notification"] = movie.Name + " has been deleted from the movies database!";
            moviesRepository.DeleteMovie(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                moviesRepository.Dispose();
                actorsRepository.Dispose();
                producersRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
