using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApiTax.Entities;
using PagedList;
using ApiTax.Models;

namespace ApiTax.Controllers
{
    public class RailWayRequestsController : Controller
    {
        private MrTripTEntities db = new MrTripTEntities();

        // GET: RailWayRequests
        public ActionResult Index(int? page, string search = "")
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            int pageSize = 15;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            if (GlobalUser.isLogin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }
            ViewBag.page_index = pageIndex;
            var railWayRequests = db.RailWayRequests.Include(r => r.Done).Include(r => r.RequestType).Include(r => r.Sex).Include(r => r.RailWayStation).Include(r => r.RailWayStation1).Include(r => r.TicketType).Include(r => r.TripType);
            return View(railWayRequests.OrderByDescending(r => r.RailWayRequestID).ToPagedList(pageIndex, pageSize));
        }

        // GET: RailWayRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RailWayRequest railWayRequest = db.RailWayRequests.Find(id);
            if (railWayRequest == null)
            {
                return HttpNotFound();
            }
            return View(railWayRequest);
        }

        // GET: RailWayRequests/Create
        public ActionResult Create()
        {
            ViewBag.DoneID = new SelectList(db.Dones, "ID", "Title");
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Title");
            ViewBag.SexCode = new SelectList(db.Sexes, "SexCode", "SexName");
            ViewBag.StartStationID = new SelectList(db.RailWayStations, "StationID", "Station");
            ViewBag.EndStationID = new SelectList(db.RailWayStations, "StationID", "Station");
            ViewBag.TicketTypeID = new SelectList(db.TicketTypes, "TicketTypeID", "Title");
            ViewBag.TripTypeID = new SelectList(db.TripTypes, "TripTypeID", "Title");
            return View();
        }

        // POST: RailWayRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RailWayRequestID,StartStationID,EndStationID,TicketTypeID,StartDate,EndDate,RationCode,SexCode,TicketCount,CoupeDoor,Mobile,DoneID,LimitTime,RequestTypeID,TripTypeID,AlternativeSexCode,message,ParentRailWayRequestID")] RailWayRequest railWayRequest)
        {
            try
            {
                var date = Request.Form["StartDatePersian"];
                railWayRequest.StartDate = utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }

            try
            {
                var date = Request.Form["EndDatePersian"];
                railWayRequest.EndDate = utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }

            if (ModelState.IsValid)
            {
                db.RailWayRequests.Add(railWayRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoneID = new SelectList(db.Dones, "ID", "Title", railWayRequest.DoneID);
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Title", railWayRequest.RequestTypeID);
            ViewBag.SexCode = new SelectList(db.Sexes, "SexCode", "SexName", railWayRequest.SexCode);
            ViewBag.StartStationID = new SelectList(db.RailWayStations, "StationID", "Station", railWayRequest.StartStationID);
            ViewBag.EndStationID = new SelectList(db.RailWayStations, "StationID", "Station", railWayRequest.EndStationID);
            ViewBag.TicketTypeID = new SelectList(db.TicketTypes, "TicketTypeID", "Title", railWayRequest.TicketTypeID);
            ViewBag.TripTypeID = new SelectList(db.TripTypes, "TripTypeID", "Title", railWayRequest.TripTypeID);
            return View(railWayRequest);
        }

        // GET: RailWayRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RailWayRequest railWayRequest = db.RailWayRequests.Find(id);
            if (railWayRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoneID = new SelectList(db.Dones, "ID", "Title", railWayRequest.DoneID);
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Title", railWayRequest.RequestTypeID);
            ViewBag.SexCode = new SelectList(db.Sexes, "SexCode", "SexName", railWayRequest.SexCode);
            ViewBag.StartStationID = new SelectList(db.RailWayStations, "StationID", "Station", railWayRequest.StartStationID);
            ViewBag.EndStationID = new SelectList(db.RailWayStations, "StationID", "Station", railWayRequest.EndStationID);
            ViewBag.TicketTypeID = new SelectList(db.TicketTypes, "TicketTypeID", "Title", railWayRequest.TicketTypeID);
            ViewBag.TripTypeID = new SelectList(db.TripTypes, "TripTypeID", "Title", railWayRequest.TripTypeID);
            return View(railWayRequest);
        }

        // POST: RailWayRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RailWayRequestID,StartStationID,EndStationID,TicketTypeID,StartDate,EndDate,RationCode,SexCode,TicketCount,CoupeDoor,Mobile,DoneID,LimitTime,RequestTypeID,TripTypeID,AlternativeSexCode,message,ParentRailWayRequestID")] RailWayRequest railWayRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(railWayRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            try
            {
                var date = Request.Form["StartDatePersian"];
                railWayRequest.StartDate = utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }

            try
            {
                var date = Request.Form["EndDatePersian"];
                railWayRequest.EndDate = utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }
            ViewBag.DoneID = new SelectList(db.Dones, "ID", "Title", railWayRequest.DoneID);
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes, "RequestTypeID", "Title", railWayRequest.RequestTypeID);
            ViewBag.SexCode = new SelectList(db.Sexes, "SexCode", "SexName", railWayRequest.SexCode);
            ViewBag.StartStationID = new SelectList(db.RailWayStations, "StationID", "Station", railWayRequest.StartStationID);
            ViewBag.EndStationID = new SelectList(db.RailWayStations, "StationID", "Station", railWayRequest.EndStationID);
            ViewBag.TicketTypeID = new SelectList(db.TicketTypes, "TicketTypeID", "Title", railWayRequest.TicketTypeID);
            ViewBag.TripTypeID = new SelectList(db.TripTypes, "TripTypeID", "Title", railWayRequest.TripTypeID);
            return View(railWayRequest);
        }

        // GET: RailWayRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RailWayRequest railWayRequest = db.RailWayRequests.Find(id);
            if (railWayRequest == null)
            {
                return HttpNotFound();
            }
            return View(railWayRequest);
        }

        // POST: RailWayRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RailWayRequest railWayRequest = db.RailWayRequests.Find(id);
            db.RailWayRequests.Remove(railWayRequest);
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
