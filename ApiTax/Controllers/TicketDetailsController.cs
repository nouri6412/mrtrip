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
    public class TicketDetailsController : Controller
    {
        private MrTripTEntities db = new MrTripTEntities();

        // GET: TicketDetails
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
            var ticketDetails = db.TicketDetails.Include(t => t.RailWayRequest);
            return View(ticketDetails.OrderByDescending(r => r.ID).ToPagedList(pageIndex, pageSize));
        }

        // GET: TicketDetails/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketDetail ticketDetail = db.TicketDetails.Find(id);
            if (ticketDetail == null)
            {
                return HttpNotFound();
            }
            return View(ticketDetail);
        }

        // GET: TicketDetails/Create
        public ActionResult Create()
        {
            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile");
            return View();
        }

        // POST: TicketDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RailWayRequestID,SellSerial,Row,TrackId,FromStationName,ExitDate,ExitDay,TrainNo,ToStationName,PassengerName,WagonName,ExitTime,WagonNo,SeatNo,ArriveTime,TicketPrice,DiscountAmount,Issuer,StationServices,TotalPrice,CancelRequest,IsCanceled,CancelDate")] TicketDetail ticketDetail)
        {
            try
            {
                var date = Request.Form["CancelDatePersian"];
                ticketDetail.CancelDate = utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }
            if (ModelState.IsValid)
            {
                db.TicketDetails.Add(ticketDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile", ticketDetail.RailWayRequestID);
            return View(ticketDetail);
        }

        // GET: TicketDetails/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketDetail ticketDetail = db.TicketDetails.Find(id);
            if (ticketDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile", ticketDetail.RailWayRequestID);
            return View(ticketDetail);
        }

        // POST: TicketDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RailWayRequestID,SellSerial,Row,TrackId,FromStationName,ExitDate,ExitDay,TrainNo,ToStationName,PassengerName,WagonName,ExitTime,WagonNo,SeatNo,ArriveTime,TicketPrice,DiscountAmount,Issuer,StationServices,TotalPrice,CancelRequest,IsCanceled,CancelDate")] TicketDetail ticketDetail)
        {
            try
            {
                var date = Request.Form["CancelDatePersian"];
                ticketDetail.CancelDate = utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }
            if (ModelState.IsValid)
            {
                db.Entry(ticketDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile", ticketDetail.RailWayRequestID);
            return View(ticketDetail);
        }

        // GET: TicketDetails/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketDetail ticketDetail = db.TicketDetails.Find(id);
            if (ticketDetail == null)
            {
                return HttpNotFound();
            }
            return View(ticketDetail);
        }

        // POST: TicketDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            TicketDetail ticketDetail = db.TicketDetails.Find(id);
            db.TicketDetails.Remove(ticketDetail);
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
