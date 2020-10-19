using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoCalidad.Models;

namespace ProyectoCalidad.Controllers
{
    public class RoutesController : Controller
    {
        private AirportApplicationEntities db = new AirportApplicationEntities();

        // GET: Routes
        public ActionResult Index()
        {
            return View(db.Rutas.ToList());
        }

        // GET: Routes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ruta ruta = db.Rutas.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            return View(ruta);
        }

        // GET: Routes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idRutaPK,codigoVuelo,companiaAerea,diaSemana,pais,arrivalDeparture,capacidadMaxima")] Ruta ruta)
        {
            if (ModelState.IsValid)
            {
                db.Rutas.Add(ruta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ruta);
        }

        // GET: Routes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ruta ruta = db.Rutas.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            return View(ruta);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idRutaPK,codigoVuelo,companiaAerea,diaSemana,pais,arrivalDeparture,capacidadMaxima")] Ruta ruta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ruta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ruta);
        }

        // GET: Routes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ruta ruta = db.Rutas.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            return View(ruta);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ruta ruta = db.Rutas.Find(id);
            db.Rutas.Remove(ruta);
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
        public JsonResult getRoutesDays()
        {
            List<DaysForDropdown> daysList =
               (from route in db.Rutas
                select new DaysForDropdown
                {
                    weekDay = route.diaSemana
                }).Distinct().ToList();

            return Json(daysList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getRoutePassengers(int routeIdFK)
        {
            var passengers = from route in db.Rutas
                             where route.idRutaPK == routeIdFK
                             select route.capacidadMaxima;

            return Json(passengers, JsonRequestBehavior.AllowGet);
        }

    }
}
