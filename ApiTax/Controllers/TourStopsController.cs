using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApiTax.Entities;
using ApiTax.Models;

namespace ApiTax.Controllers
{
    public class TourStopsController : Controller
    {
        private MrTripEntities db = new MrTripEntities();

        // GET: TourStops
        public ActionResult Index(long tour_id=0)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            Tour tour = db.Tours.Find(tour_id);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            var tourStops = db.TourStops.Where(r=>r.TourId==tour_id).Include(t => t.Hotel).Include(t => t.Location).Include(t => t.LocCity).Include(t => t.Station).Include(t => t.Station1).Include(t => t.Tour).Include(t => t.TourStopType).Include(t => t.TransportCompany);
            ViewBag.tour_id = tour_id;
            return View(tourStops.ToList());
        }

        // GET: TourStops/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourStop tourStop = db.TourStops.Find(id);
            if (tourStop == null)
            {
                return HttpNotFound();
            }
            return View(tourStop);
        }

        // GET: TourStops/Create
        public ActionResult Create(long tour_id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Title");
            ViewBag.LocationId = new SelectList(db.Locations.OrderBy(r => r.Title), "Id", "Title");
            ViewBag.CityId = new SelectList(db.LocCities, "Id", "Title");
            ViewBag.DepartureStationId = new SelectList(db.Stations, "Id", "Title");
            ViewBag.ArrivalStationId = new SelectList(db.Stations, "Id", "Title");
            ViewBag.StopTypeId = new SelectList(db.TourStopTypes, "Id", "Titile");
            ViewBag.TransportCompanyId = new SelectList(db.TransportCompanies, "Id", "Title");
            ViewBag.tour_id = tour_id;

            Tour tour = db.Tours.Find(tour_id);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            ViewBag.ErrorMessage = "";
            return View();
        }

        // POST: TourStops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,TourId,CityId,LocationId,StopTypeId,StopOrder,Nights,HotelId,TransportCompanyId,DepartureDate,DepartureDateFa,DepartureTime,DepartureStationId,ArrivalTime,ArrivalStationId,Duration,WaitDuration")] TourStop tourStop)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            try
            {
                var date = Request.Form["DepartureDatePersian"];
                tourStop.DepartureDate = utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }

            Tour tour = db.Tours.Find(tourStop.TourId);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                tourStop.DepartureDateFa = utility.ToPersian(tourStop.DepartureDate.ToShortDateString());
                db.TourStops.Add(tourStop);
                db.SaveChanges();

                return RedirectToAction("Edit", "Tours", new { id = tourStop.TourId, type = "day" });
            }

            ViewBag.ErrorMessage = "به خطاها توجه نمائید";
            ViewBag.tour_id = tourStop.TourId;


            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Title", tourStop.HotelId);
            ViewBag.LocationId = new SelectList(db.Locations.OrderBy(r => r.Title), "Id", "Title", tourStop.LocationId);
            ViewBag.CityId = new SelectList(db.LocCities, "Id", "Title", tourStop.CityId);
            ViewBag.DepartureStationId = new SelectList(db.Stations, "Id", "Title", tourStop.DepartureStationId);
            ViewBag.ArrivalStationId = new SelectList(db.Stations, "Id", "Title", tourStop.ArrivalStationId);
            ViewBag.StopTypeId = new SelectList(db.TourStopTypes, "Id", "Titile", tourStop.StopTypeId);
            ViewBag.TransportCompanyId = new SelectList(db.TransportCompanies, "Id", "Title", tourStop.TransportCompanyId);
            return View(tourStop);
        }

        // GET: TourStops/Edit/5
        public ActionResult Edit(long? id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User); 

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourStop tourStop = db.TourStops.Find(id);
            if (tourStop == null)
            {
                return HttpNotFound();
            }

            Tour tour = db.Tours.Find(tourStop.TourId);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            ViewBag.ErrorMessage = "";
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Title", tourStop.HotelId);
            ViewBag.LocationId = new SelectList(db.Locations.OrderBy(r=>r.Title), "Id", "Title", tourStop.LocationId);
            ViewBag.CityId = new SelectList(db.LocCities, "Id", "Title", tourStop.CityId);
            ViewBag.DepartureStationId = new SelectList(db.Stations, "Id", "Title", tourStop.DepartureStationId);
            ViewBag.ArrivalStationId = new SelectList(db.Stations, "Id", "Title", tourStop.ArrivalStationId);
            ViewBag.StopTypeId = new SelectList(db.TourStopTypes, "Id", "Titile", tourStop.StopTypeId);
            ViewBag.TransportCompanyId = new SelectList(db.TransportCompanies, "Id", "Title", tourStop.TransportCompanyId);
            return View(tourStop);
        }

        // POST: TourStops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TourId,CityId,LocationId,StopTypeId,StopOrder,Nights,HotelId,TransportCompanyId,DepartureDate,DepartureDateFa,DepartureTime,DepartureStationId,ArrivalTime,ArrivalStationId,Duration,WaitDuration")] TourStop tourStop)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            try
            {
                var date = Request.Form["DepartureDatePersian"];
                tourStop.DepartureDate = utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }

            Tour tour = db.Tours.Find(tourStop.TourId);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                tourStop.DepartureDateFa = utility.ToPersian(tourStop.DepartureDate.ToShortDateString());
                db.Entry(tourStop).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Edit", "Tours", new { id = tourStop.TourId, type = "day" });
            }
            ViewBag.ErrorMessage = "به خطاها توجه نمائید";
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Title", tourStop.HotelId);
            ViewBag.LocationId = new SelectList(db.Locations.OrderBy(r => r.Title), "Id", "Title", tourStop.LocationId);
            ViewBag.CityId = new SelectList(db.LocCities, "Id", "Title", tourStop.CityId);
            ViewBag.DepartureStationId = new SelectList(db.Stations, "Id", "Title", tourStop.DepartureStationId);
            ViewBag.ArrivalStationId = new SelectList(db.Stations, "Id", "Title", tourStop.ArrivalStationId);
            ViewBag.TourId = new SelectList(db.Tours, "Id", "Title", tourStop.TourId);
            ViewBag.StopTypeId = new SelectList(db.TourStopTypes, "Id", "Titile", tourStop.StopTypeId);
            ViewBag.TransportCompanyId = new SelectList(db.TransportCompanies, "Id", "Title", tourStop.TransportCompanyId);
            return View(tourStop);
        }

        // GET: TourStops/Delete/5
        public ActionResult Delete(long? id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourStop tourStop = db.TourStops.Find(id);

            Tour tour = db.Tours.Find(tourStop.TourId);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            if (tourStop == null)
            {
                return HttpNotFound();
            }
            return View(tourStop);
        }

        // POST: TourStops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            TourStop tourStop = db.TourStops.Find(id);

            Tour tour = db.Tours.Find(tourStop.TourId);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            db.TourStops.Remove(tourStop);
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
