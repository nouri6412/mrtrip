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
using Newtonsoft.Json;

namespace ApiTax.Controllers
{
    public class TourCouponsController : Controller
    {
        private MrTripEntities db = new MrTripEntities();

        // GET: TourAgencyCosts
        public ActionResult Index(long tour_id = 0)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            Tour tour = db.Tours.Find(tour_id);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }
            ViewBag.tour_id = tour_id;
            var tourCoupons = db.Tours.Where(r => r.Id == tour_id).Include(t => t.Coupons).FirstOrDefault().Coupons;
            return View(tourCoupons.ToList());
        }


        // GET: TourAgencyCosts/Create
        public ActionResult Create(long tour_id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            Tour tour = db.Tours.Find(tour_id);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }
            ViewBag.tour_id = tour_id;
            ViewBag.CouponId = new SelectList(db.Coupons, "Id", "Code");
            ViewBag.ErrorMessage = "";
            return View();
        }

        // POST: TourAgencyCosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TourId,CouponId")] TourCoupon TourCoupon)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

           
            if ( GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            var coupon = db.Coupons.FirstOrDefault(r=>r.Id== TourCoupon.CouponId);

            if (ModelState.IsValid)
            {
                db.Tours.FirstOrDefault(r=>r.Id== TourCoupon.TourId).Coupons.Add(coupon);
                db.SaveChanges();
                return RedirectToAction("Edit", "Tours", new { id = TourCoupon.TourId, type = "coupon" });
            }
            ViewBag.ErrorMessage = "به خطاها توجه نمائید";
            ViewBag.CouponId = new SelectList(db.Coupons, "Id", "Code");
            ViewBag.tour_id = TourCoupon.TourId;
            return View(TourCoupon);
        }
        // GET: TourAgencyCosts/Delete/5
        public ActionResult Delete(int tour_id,int coupon_id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);


            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            Tour tour = db.Tours.Find(tour_id);
            var coupon = db.Coupons.FirstOrDefault(r => r.Id == coupon_id);
            tour.Coupons.Remove(coupon);
            db.SaveChanges();
            return RedirectToAction("Edit", "Tours", new { id = tour_id, type = "coupon" });
        }

        // POST: TourAgencyCosts/Delete/5
        [HttpPost, ActionName("Delete")]
    
        public ActionResult DeleteConfirmed(int tour_id, int coupon_id)
        {

            Tour tour = db.Tours.Find(tour_id);
            var coupon = db.Coupons.FirstOrDefault(r => r.Id == coupon_id);
            tour.Coupons.Remove(coupon);
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
public class TourCoupon
{
    public int TourId { get; set; }
    public int CouponId { get; set; }
}
