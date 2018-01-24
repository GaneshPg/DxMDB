using DxMDB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DxMDB.Controllers
{
    public class HomeController : Controller
    {
        private MovieDBContext db;

        public HomeController()
        {
            db = new MovieDBContext();
        }

        public ActionResult Index()
        {
            return View(db.Movies);
        }

        public ActionResult Details(int id)
        {
            return View(db.Movies.Single<Movie>(m => m.Id == id));
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Producers = db.Producers;
            ViewBag.Actors = db.Actors;
            return View(db.Movies.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(FormCollection formCollection)
        {
            Movie movie = new Movie();
            movie.Name = formCollection.Get("Name");
            movie.Plot = formCollection["Plot"];
            movie.Yor = int.Parse(formCollection["Yor"]);
            string pname = Request["producer"];
            Producer p = db.Producers.FirstOrDefault<Producer>(producer => producer.Name == pname);
            movie.ProducerId = p.Id;
            movie.Id = int.Parse(formCollection["id"]);
            db.Entry(movie).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            ViewBag.Producers = db.Producers;
            ViewBag.Actors = db.Actors;

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            Movie movie = new Movie();
            movie.Name = formCollection.Get("Name");
            movie.Plot = formCollection["Plot"];
            movie.Yor = int.Parse(formCollection["Yor"]);
            string pname = Request["producer"];
            Producer p = db.Producers.FirstOrDefault<Producer>(producer => producer.Name == pname);
            movie.ProducerId = p.Id;
            string[] actors = formCollection["actor"].Split(',');
            foreach (string actor in actors)
            {
                Actor a = db.Actors.FirstOrDefault<Actor>(act => act.Name == actor);
                
            }
            db.Movies.Add(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (true)
            {
                db.Entry(db.Movies.Find(id)).State = EntityState.Deleted;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}