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
    public class TicketInfoesController : Controller
    {
        private MrTripTEntities db = new MrTripTEntities();

        // GET: TicketInfoes
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
            var ticketInfoes = db.TicketInfoes.Include(t => t.RailWayRequest);
            return View(ticketInfoes.OrderByDescending(r => r.ID).ToPagedList(pageIndex, pageSize));
        }

        // GET: TicketInfoes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketInfo ticketInfo = db.TicketInfoes.Find(id);
            if (ticketInfo == null)
            {
                return HttpNotFound();
            }
            return View(ticketInfo);
        }

        // GET: TicketInfoes/Create
        public ActionResult Create()
        {
            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile");
            return View();
        }

        // POST: TicketInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RailWayRequestID,SellSerial,FromStationName,ToStationName,ShExiteDate,ExiteTime,Degree,WagonNo,CompartmentNo,SexName,TrainNo,WagonTypeName,RationName,Status,Message,Created,Updated,TrackID")] TicketInfo ticketInfo)
        {
            if (ModelState.IsValid)
            {
                db.TicketInfoes.Add(ticketInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r=>r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile", ticketInfo.RailWayRequestID);
            return View(ticketInfo);
        }

        // GET: TicketInfoes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketInfo ticketInfo = db.TicketInfoes.Find(id);
            if (ticketInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile", ticketInfo.RailWayRequestID);
            return View(ticketInfo);
        }

        // POST: TicketInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RailWayRequestID,SellSerial,FromStationName,ToStationName,ShExiteDate,ExiteTime,Degree,WagonNo,CompartmentNo,SexName,TrainNo,WagonTypeName,RationName,Status,Message,Created,Updated,TrackID")] TicketInfo ticketInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile", ticketInfo.RailWayRequestID);
            return View(ticketInfo);
        }

        // GET: TicketInfoes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketInfo ticketInfo = db.TicketInfoes.Find(id);
            if (ticketInfo == null)
            {
                return HttpNotFound();
            }
            return View(ticketInfo);
        }

        // POST: TicketInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TicketInfo ticketInfo = db.TicketInfoes.Find(id);
            db.TicketInfoes.Remove(ticketInfo);
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
