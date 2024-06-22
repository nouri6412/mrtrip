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
    public class RWWsController : Controller
    {
        private MrTripTEntities db = new MrTripTEntities();

        // GET: RWWs
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
            var rWWs = db.RWWs.Include(r => r.RailWayRequest).Include(r => r.Scheduling).Include(r => r.Wagon).Where(r=>r.WRailWayRequestID==wid);
            return View(rWWs.OrderByDescending(r => r.WID).ToPagedList(pageIndex, pageSize));
        }

        // GET: RWWs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RWW rWW = db.RWWs.Find(id);
            if (rWW == null)
            {
                return HttpNotFound();
            }
            return View(rWW);
        }

        // GET: RWWs/Create
        public ActionResult Create(int wid=0)
        {
            ViewBag.wid = wid;
            ViewBag.WSchedulingID = new SelectList(db.Schedulings, "ID", "ID");
            ViewBag.WWagonTypeID = new SelectList(db.Wagons, "WagonTypeID", "WagonName");
            return View();
        }

        // POST: RWWs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WID,WRailWayRequestID,WSchedulingID,WWagonTypeID,WPriority")] RWW rWW)
        {
            if (ModelState.IsValid)
            {
                db.RWWs.Add(rWW);
                db.SaveChanges();
                return RedirectToAction("Index", new {wid= rWW.WRailWayRequestID });
            }
            ViewBag.wid = rWW.WRailWayRequestID;
            ViewBag.WSchedulingID = new SelectList(db.Schedulings, "ID", "ID", rWW.WSchedulingID);
            ViewBag.WWagonTypeID = new SelectList(db.Wagons, "WagonTypeID", "WagonName", rWW.WWagonTypeID);
            return View(rWW);
        }

        // GET: RWWs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RWW rWW = db.RWWs.Find(id);
            if (rWW == null)
            {
                return HttpNotFound();
            }
            ViewBag.WSchedulingID = new SelectList(db.Schedulings, "ID", "ID", rWW.WSchedulingID);
            ViewBag.WWagonTypeID = new SelectList(db.Wagons, "WagonTypeID", "WagonName", rWW.WWagonTypeID);
            return View(rWW);
        }

        // POST: RWWs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WID,WRailWayRequestID,WSchedulingID,WWagonTypeID,WPriority")] RWW rWW)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rWW).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { wid = rWW.WRailWayRequestID });
            }
            ViewBag.WRailWayRequestID = new SelectList(db.RailWayRequests.OrderByDescending(r => r.RailWayRequestID).Take(50).ToList(), "RailWayRequestID", "Mobile", rWW.WRailWayRequestID);
            ViewBag.WSchedulingID = new SelectList(db.Schedulings, "ID", "ID", rWW.WSchedulingID);
            ViewBag.WWagonTypeID = new SelectList(db.Wagons, "WagonTypeID", "WagonName", rWW.WWagonTypeID);
            return View(rWW);
        }

        // GET: RWWs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RWW rWW = db.RWWs.Find(id);
            if (rWW == null)
            {
                return HttpNotFound();
            }
            return View(rWW);
        }

        // POST: RWWs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RWW rWW = db.RWWs.Find(id);
            db.RWWs.Remove(rWW);
            db.SaveChanges();
            return RedirectToAction("Index", new { wid = rWW.WRailWayRequestID });
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
