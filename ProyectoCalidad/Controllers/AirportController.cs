using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using ProyectoCalidad.Models;

namespace ProyectoCalidad.Controllers
{
    public class AirportController : Controller
    {
        private AirportApplicationEntities db = new AirportApplicationEntities();

        // GET: Airport
        public ActionResult Index()
        {
            return View(db.Aeropuertos.ToList());
        }

        // GET: Airport/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aeropuerto aeropuerto = db.Aeropuertos.Find(id);
            if (aeropuerto == null)
            {
                return HttpNotFound();
            }
            return View(aeropuerto);
        }

        // GET: Airport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Airport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codigoAeropuertoPK,nombreAeropuerto,direccion,habilitado")] Aeropuerto aeropuerto)
        {
            if (ModelState.IsValid)
            {
                db.Aeropuertos.Add(aeropuerto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aeropuerto);
        }

        // GET: Airport/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aeropuerto aeropuerto = db.Aeropuertos.Find(id);
            if (aeropuerto == null)
            {
                return HttpNotFound();
            }
            return View(aeropuerto);
        }

        // POST: Airport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codigoAeropuertoPK,nombreAeropuerto,direccion,habilitado")] Aeropuerto aeropuerto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aeropuerto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aeropuerto);
        }

        // GET: Airport/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aeropuerto aeropuerto = db.Aeropuertos.Find(id);
            if (aeropuerto == null)
            {
                return HttpNotFound();
            }
            return View(aeropuerto);
        }

        // POST: Airport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Aeropuerto aeropuerto = db.Aeropuertos.Find(id);
            db.Aeropuertos.Remove(aeropuerto);
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

        //Methods and functions created by the team
        public JsonResult getAirports()
        {
            List<AirportsForDropdown> airportsList =
               (from airport in db.Aeropuertos
                select new AirportsForDropdown
                {
                    airportCode = airport.codigoAeropuertoPK,
                    airportName = airport.nombreAeropuerto

                }).ToList();

            return Json(airportsList, JsonRequestBehavior.AllowGet);
        }

    }
}
