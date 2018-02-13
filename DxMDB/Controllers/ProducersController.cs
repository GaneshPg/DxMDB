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
using Newtonsoft.Json.Linq;

namespace DxMDB.Controllers
{
    public class ProducersController : Controller
    {
        private ProducersRepository repository = new ProducersRepository();

        // GET: Producers
        public ActionResult Index()
        {
            return View(repository.GetProducers());
        }

        // GET: Producers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = repository.GetProducerById(id ?? 1);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        public ActionResult ModalCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModalUpdateDB(string Name, string Gender, string DateOfBirth, string Bio)
        {
            Producer producer;
            try
            {
                producer = new Producer { Name = Name, Gender = Gender, DateOfBirth = DateTime.Parse(DateOfBirth), Bio = Bio };
            }
            catch (Exception e)
            {
                producer = new Producer { Name = Name, Gender = Gender, Bio = Bio };
            }

            repository.AddProducer(producer);

            JObject jObject = JObject.FromObject(producer);

            return Content(jObject.ToString(), "application/json");
        }

        // GET: Producers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Gender,DateOfBirth,Bio")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                repository.AddProducer(producer);
                TempData["Notification"] = producer.Name + " has been added succesfully to the producers database!";
                return RedirectToAction("Index");
            }

            return View(producer);
        }

        // GET: Producers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = repository.GetProducerById(id ?? 1);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // POST: Producers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Gender,DateOfBirth,Bio")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                repository.UpdateProducer(producer);
                TempData["Notification"] = producer.Name + " has been edited succesfully!";
                return RedirectToAction("Index");
            }
            return View(producer);
        }

        // GET: Producers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = repository.GetProducerById(id ?? 1);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producer producer = repository.GetProducerById(id);
            repository.DeleteProducer(id);
            TempData["Notification"] = producer.Name + " has been deleted from the producers database!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
