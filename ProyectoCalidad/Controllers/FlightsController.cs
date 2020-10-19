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
    public class FlightsController : Controller
    {
        private AirportApplicationEntities db = new AirportApplicationEntities();

        // GET: Flights
        public ActionResult Index()
        {
            var vuelos = db.Vuelos.Include(v => v.Aeropuerto).Include(v => v.Ruta);
            return View(vuelos.ToList());
        }

        // GET: Flights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vuelo vuelo = db.Vuelos.Find(id);
            if (vuelo == null)
            {
                return HttpNotFound();
            }
            return View(vuelo);
        }

        // GET: Flights/Create
        public ActionResult Create()
        {
            ViewBag.codigoAeropuertoFK = new SelectList(db.Aeropuertos, "codigoAeropuertoPK", "nombreAeropuerto");
            ViewBag.idRutaFK = new SelectList(db.Rutas, "idRutaPK", "codigoVuelo");
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idVueloPK,idRutaFK,codigoAeropuertoFK,codigoVuelo,fecha,cantidadRealPasajeros")] Vuelo vuelo)
        {
            if (ModelState.IsValid)
            {
                db.Vuelos.Add(vuelo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.codigoAeropuertoFK = new SelectList(db.Aeropuertos, "codigoAeropuertoPK", "nombreAeropuerto", vuelo.codigoAeropuertoFK);
            ViewBag.idRutaFK = new SelectList(db.Rutas, "idRutaPK", "codigoVuelo", vuelo.idRutaFK);
            return View(vuelo);
        }

        // GET: Flights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vuelo vuelo = db.Vuelos.Find(id);
            if (vuelo == null)
            {
                return HttpNotFound();
            }
            ViewBag.codigoAeropuertoFK = new SelectList(db.Aeropuertos, "codigoAeropuertoPK", "nombreAeropuerto", vuelo.codigoAeropuertoFK);
            ViewBag.idRutaFK = new SelectList(db.Rutas, "idRutaPK", "codigoVuelo", vuelo.idRutaFK);
            return View(vuelo);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idVueloPK,idRutaFK,codigoAeropuertoFK,codigoVuelo,fecha,cantidadRealPasajeros")] Vuelo vuelo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vuelo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.codigoAeropuertoFK = new SelectList(db.Aeropuertos, "codigoAeropuertoPK", "nombreAeropuerto", vuelo.codigoAeropuertoFK);
            ViewBag.idRutaFK = new SelectList(db.Rutas, "idRutaPK", "codigoVuelo", vuelo.idRutaFK);
            return View(vuelo);
        }

        // GET: Flights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vuelo vuelo = db.Vuelos.Find(id);
            if (vuelo == null)
            {
                return HttpNotFound();
            }
            return View(vuelo);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vuelo vuelo = db.Vuelos.Find(id);
            db.Vuelos.Remove(vuelo);
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

        public JsonResult getAvailableDates()
        {
            List<DatesForDropdown> datesList =
                (from vuelo in db.Vuelos
                 orderby vuelo.fecha
                 select new DatesForDropdown
                 {
                     date = vuelo.fecha

                 }).Distinct().ToList();


            foreach (var item in datesList)
            {
                item.dateToShow = item.date.ToString("dd/MMM/yyyy");
            }

            return Json(datesList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAvailableDatesWithDependencies(String dateTime)
        {
            var dateToCompare = DateTime.Parse(dateTime);
            List<DatesForDropdown> datesList =
                (from vuelo in db.Vuelos
                 where vuelo.fecha > dateToCompare
                 orderby vuelo.fecha
                 select new DatesForDropdown
                 {
                     date = vuelo.fecha

                 }).Distinct().ToList();

            foreach (var item in datesList)
            {
                item.dateToShow = item.date.ToString("dd/MMM/yyyy");
            }

            return Json(datesList, JsonRequestBehavior.AllowGet);
        }

        public List<FlightsForQueries> getAllFlights()
        {
            List<FlightsForQueries> flightsList =
              (from vuelo in db.Vuelos
               select new FlightsForQueries
               {
                   flightId = vuelo.idVueloPK,
                   routeCode = vuelo.idRutaFK,
                   airportCode = vuelo.codigoAeropuertoFK,
                   flightCode = vuelo.codigoVuelo,
                   date = vuelo.fecha,
                   realPaasengers = vuelo.cantidadRealPasajeros,
                   weekDay = vuelo.Ruta.diaSemana,
                   arrivalDeparture = vuelo.Ruta.arrivalDeparture,
                   airportName = vuelo.Aeropuerto.nombreAeropuerto

               }).ToList();

            foreach (var flight in flightsList)
            {
                flight.dateToShow = flight.date.ToString("dd/MMM/yyyy");
            }

            return flightsList;
        }

        public JsonResult getFilteredFlights(String airport, String arrivalDeparture, String dayOption, String startDate, String endDate, String radioOptionSelected)
        {
            List<FlightsForQueries> flightsList = getAllFlights();

            if (airport != "NA")
            {
                flightsList = flightsList.Where(tuple => tuple.airportCode == airport).ToList();
            }

            if (arrivalDeparture != "NA")
            {
                flightsList = flightsList.Where(tuple => tuple.arrivalDeparture == arrivalDeparture).ToList();
            }

            if(startDate != "NA" && endDate != "NA")
            {
                flightsList = flightsList.Where(tuple => tuple.date >= Convert.ToDateTime(changeDate(startDate)) && tuple.date <= Convert.ToDateTime(changeDate(endDate))).ToList();
            }

            if(radioOptionSelected == "1")
            {
                flightsList = flightsList.Where(tuple => tuple.weekDay == dayOption).ToList();
            }

            if(radioOptionSelected == "2")
            {
                flightsList = flightsList.Where(tuple => tuple.weekDay == "Lunes" || tuple.weekDay == "Martes" || tuple.weekDay == "Miercoles" || tuple.weekDay == "Jueves" || tuple.weekDay == "Viernes").ToList();
            }
            if (radioOptionSelected == "3")
            {
               flightsList = flightsList.Where(tuple => tuple.weekDay == "Sabado" || tuple.weekDay == "Domingo" || tuple.weekDay == "Sábado").ToList();
            }

            return Json(flightsList, JsonRequestBehavior.AllowGet);
        }
        private String changeDate (String date)
        {
            
            if (date.Contains("ene."))
            {
                return date.Replace("ene.", "01");
            }
            else if (date.Contains("feb."))
            {
                return date.Replace("feb.", "02");
            }
            else if (date.Contains("mar."))
            {
                return date.Replace("mar.", "03");
            }
            else if (date.Contains("abr."))
            {
                return date.Replace("abr.", "04");
            }
            else if (date.Contains("may."))
            {
                return date.Replace("may.", "05");
            }
            else if (date.Contains("jun."))
            {
                return date.Replace("jun.", "06");
            }
            else if (date.Contains("jul."))
            {
                return date.Replace("jul.", "07");
            }
            else if (date.Contains("ago."))
            {
                return date.Replace("ago.", "08");
            }
            else if (date.Contains("sep."))
            {
                return date.Replace("sep.", "09");
            }
            else if (date.Contains("oct."))
            {
                return date.Replace("oct.", "10");
            }
            else if (date.Contains("nov."))
            {
                return date.Replace("nov.", "11");
            }
            if (date.Contains("dic."))
            {
                return date.Replace("dic.", "12");
            }

            return date;
        }
    }

}
