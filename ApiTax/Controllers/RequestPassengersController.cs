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
    public class RequestPassengersController : Controller
    {
        private MrTripTEntities db = new MrTripTEntities();

        // GET: RequestPassengers
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
            var requestPassengers = db.RequestPassengers.Include(r => r.Passenger).Include(r => r.PassengerService).Include(r => r.RailWayRequest);
            return View(requestPassengers.OrderByDescending(r => r.RequestPassengerID).ToPagedList(pageIndex, pageSize));
        }

        // GET: RequestPassengers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestPassenger requestPassenger = db.RequestPassengers.Find(id);
            if (requestPassenger == null)
            {
                return HttpNotFound();
            }
            return View(requestPassenger);
        }

        // GET: RequestPassengers/Create
        public ActionResult Create()
        {
            ViewBag.PassengerID = new SelectList(db.Passengers, "PassengerID", "Name");
            ViewBag.ServiceID = new SelectList(db.PassengerServices, "serviceID", "Title");
            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile");
            return View();
        }

        // POST: RequestPassengers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestPassengerID,PassengerID,ServiceID,RailWayRequestID")] RequestPassenger requestPassenger)
        {
            if (ModelState.IsValid)
            {
                db.RequestPassengers.Add(requestPassenger);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PassengerID = new SelectList(db.Passengers, "PassengerID", "Name", requestPassenger.PassengerID);
            ViewBag.ServiceID = new SelectList(db.PassengerServices, "serviceID", "Title", requestPassenger.ServiceID);
            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile", requestPassenger.RailWayRequestID);
            return View(requestPassenger);
        }

        // GET: RequestPassengers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestPassenger requestPassenger = db.RequestPassengers.Find(id);
            if (requestPassenger == null)
            {
                return HttpNotFound();
            }
            ViewBag.PassengerID = new SelectList(db.Passengers, "PassengerID", "Name", requestPassenger.PassengerID);
            ViewBag.ServiceID = new SelectList(db.PassengerServices, "serviceID", "Title", requestPassenger.ServiceID);
            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile", requestPassenger.RailWayRequestID);
            return View(requestPassenger);
        }

        // POST: RequestPassengers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestPassengerID,PassengerID,ServiceID,RailWayRequestID")] RequestPassenger requestPassenger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestPassenger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PassengerID = new SelectList(db.Passengers, "PassengerID", "Name", requestPassenger.PassengerID);
            ViewBag.ServiceID = new SelectList(db.PassengerServices, "serviceID", "Title", requestPassenger.ServiceID);
            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile", requestPassenger.RailWayRequestID);
            return View(requestPassenger);
        }

        // GET: RequestPassengers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestPassenger requestPassenger = db.RequestPassengers.Find(id);
            if (requestPassenger == null)
            {
                return HttpNotFound();
            }
            return View(requestPassenger);
        }

        // POST: RequestPassengers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestPassenger requestPassenger = db.RequestPassengers.Find(id);
            db.RequestPassengers.Remove(requestPassenger);
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
