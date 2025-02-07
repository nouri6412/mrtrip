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
    public class RWsController : Controller
    {
        private MrTripTEntities db = new MrTripTEntities();

        // GET: RWs
        public ActionResult Index(int? page,int wid=0, string search = "")
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            int pageSize = 50000;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            if (GlobalUser.isLogin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }
            ViewBag.page_index = pageIndex;
            ViewBag.wid = wid;
            var rWS = db.RWS.Include(r => r.GoReturn).Include(r => r.RailWayRequest).Include(r => r.Scheduling).Where(r=>r.RailWayRequestID==wid);
            return View(rWS.OrderByDescending(r => r.ID).ToPagedList(pageIndex, pageSize));
        }

        // GET: RWs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RW rW = db.RWS.Find(id);
            if (rW == null)
            {
                return HttpNotFound();
            }
            return View(rW);
        }

        // GET: RWs/Create
        public ActionResult Create(int wid=0)
        {
            ViewBag.GoReturnID = new SelectList(db.GoReturns, "ID", "Ttitle");
            ViewBag.SchedulingID = new SelectList(db.Schedulings, "ID", "TrainNumber");
            
            ViewBag.wid = wid;
            return View();
        }

        // POST: RWs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RailWayRequestID,SchedulingID,GoReturnID,Priority")] RW rW)
        {
            if (ModelState.IsValid)
            {
                db.RWS.Add(rW);
                db.SaveChanges();
                return RedirectToAction("Index", new { wid = rW.RailWayRequestID });
            }

            ViewBag.GoReturnID = new SelectList(db.GoReturns, "ID", "Ttitle", rW.GoReturnID);
            ViewBag.SchedulingID = new SelectList(db.Schedulings, "ID", "TrainNumber", rW.SchedulingID);
            ViewBag.wid = rW.RailWayRequestID;
            return View(rW);
        }

        // GET: RWs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RW rW = db.RWS.Find(id);
            if (rW == null)
            {
                return HttpNotFound();
            }
            ViewBag.GoReturnID = new SelectList(db.GoReturns, "ID", "Ttitle", rW.GoReturnID);
            ViewBag.SchedulingID = new SelectList(db.Schedulings, "ID", "TrainNumber", rW.SchedulingID);
            return View(rW);
        }

        // POST: RWs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RailWayRequestID,SchedulingID,GoReturnID,Priority")] RW rW)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rW).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { wid=rW.RailWayRequestID});
            }
            ViewBag.GoReturnID = new SelectList(db.GoReturns, "ID", "Ttitle", rW.GoReturnID);
            ViewBag.SchedulingID = new SelectList(db.Schedulings, "ID", "TrainNumber", rW.SchedulingID);
            return View(rW);
        }

        // GET: RWs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RW rW = db.RWS.Find(id);
            if (rW == null)
            {
                return HttpNotFound();
            }
            return View(rW);
        }

        // POST: RWs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RW rW = db.RWS.Find(id);
            db.RWS.Remove(rW);
            db.SaveChanges();
            return RedirectToAction("Index", new { wid = rW.RailWayRequestID });
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
