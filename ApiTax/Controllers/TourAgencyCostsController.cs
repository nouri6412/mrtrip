using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApiTax.Models;
using Newtonsoft.Json;

namespace ApiTax.Controllers
{
    public class TourAgencyCostsController : Controller
    {
        private dbEntities db = new dbEntities();

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
            var tourAgencyCosts = db.TourAgencyCosts.Where(r => r.TourId == tour_id).Include(t => t.Agency).Include(t => t.Currency).Include(t => t.HotelRoomType).Include(t => t.Tour);
            return View(tourAgencyCosts.ToList());
        }

        // GET: TourAgencyCosts/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourAgencyCost tourAgencyCost = db.TourAgencyCosts.Find(id);
            if (tourAgencyCost == null)
            {
                return HttpNotFound();
            }
            return View(tourAgencyCost);
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
            ViewBag.AgencyId = new SelectList(db.Agencies, "Id", "Name");
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "Title");
            ViewBag.RoomTypeId = new SelectList(db.HotelRoomTypes, "Id", "Title");
            ViewBag.ErrorMessage = "";
            return View();
        }

        // POST: TourAgencyCosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TourId,AgencyId,RoomTypeId,Price,UserDiscount,AffiliateDiscount,FullDiscount,PackageNumber,CurrencyId,CurrencyPrice")] TourAgencyCost tourAgencyCost)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            Tour tour = db.Tours.Find(tourAgencyCost.TourId);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            if (GlobalUser.isAdmin==false)
            {
                var ag = db.Agencies.FirstOrDefault(r=>r.UserId== GlobalUser.CurrentUser.Id);
                if(ag != null)
                {
                    tourAgencyCost.AgencyId = ag.Id;

                    try
                    {
                        tourAgencyCost.UserDiscount = (ag.UserDiscount * tourAgencyCost.Price) / 100;
                        tourAgencyCost.AffiliateDiscount = (ag.UserDiscount * tourAgencyCost.Price) / 100;
                        tourAgencyCost.FullDiscount = tourAgencyCost.UserDiscount + tourAgencyCost.AffiliateDiscount;
                    }
                    catch { }
                }
            }

            if (GlobalUser.isAdmin == false)
            {

            }

            if (ModelState.IsValid)
            {
                db.TourAgencyCosts.Add(tourAgencyCost);
                db.SaveChanges();
                return RedirectToAction("Edit", "Tours", new { id = tourAgencyCost.TourId, type = "cost" });
            }
            ViewBag.ErrorMessage = "به خطاها توجه نمائید";
            ViewBag.AgencyId = new SelectList(db.Agencies, "Id", "Name", tourAgencyCost.AgencyId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "Title", tourAgencyCost.CurrencyId);
            ViewBag.RoomTypeId = new SelectList(db.HotelRoomTypes, "Id", "Title", tourAgencyCost.RoomTypeId);
            ViewBag.tour_id = tourAgencyCost.TourId;
            return View(tourAgencyCost);
        }

        // GET: TourAgencyCosts/Edit/5
        public ActionResult Edit(long? id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User); 

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TourAgencyCost tourAgencyCost = db.TourAgencyCosts.Find(id);
            if (tourAgencyCost == null)
            {
                return HttpNotFound();
            }

            Tour tour = db.Tours.Find(tourAgencyCost.TourId);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }
            ViewBag.ErrorMessage = "";
            ViewBag.AgencyId = new SelectList(db.Agencies, "Id", "Name", tourAgencyCost.AgencyId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "Title", tourAgencyCost.CurrencyId);
            ViewBag.RoomTypeId = new SelectList(db.HotelRoomTypes, "Id", "Title", tourAgencyCost.RoomTypeId);
            ViewBag.TourId = new SelectList(db.Tours, "Id", "Title", tourAgencyCost.TourId);
            ViewBag.tour_id = tourAgencyCost.TourId;
            return View(tourAgencyCost);
        }

        // POST: TourAgencyCosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TourId,AgencyId,RoomTypeId,Price,UserDiscount,AffiliateDiscount,FullDiscount,PackageNumber,CurrencyId,CurrencyPrice")] TourAgencyCost tourAgencyCost)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            Tour tour = db.Tours.Find(tourAgencyCost.TourId);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }


            if (GlobalUser.isAdmin == false)
            {
                var ag = db.Agencies.FirstOrDefault(r => r.UserId == GlobalUser.CurrentUser.Id);
                if (ag != null)
                {
                    tourAgencyCost.AgencyId = ag.Id;

                    try
                    {
                        tourAgencyCost.UserDiscount = (ag.UserDiscount * tourAgencyCost.Price) / 100;
                        tourAgencyCost.AffiliateDiscount = (ag.UserDiscount * tourAgencyCost.Price) / 100;
                        tourAgencyCost.FullDiscount = tourAgencyCost.UserDiscount + tourAgencyCost.AffiliateDiscount;
                    }
                    catch { }
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(tourAgencyCost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Tours", new { id = tourAgencyCost.TourId, type = "cost" });
            }
            ViewBag.ErrorMessage = "به خطاها توجه نمائید";
            ViewBag.AgencyId = new SelectList(db.Agencies, "Id", "Name", tourAgencyCost.AgencyId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "Title", tourAgencyCost.CurrencyId);
            ViewBag.RoomTypeId = new SelectList(db.HotelRoomTypes, "Id", "Title", tourAgencyCost.RoomTypeId);
            ViewBag.TourId = new SelectList(db.Tours, "Id", "Title", tourAgencyCost.TourId);
            ViewBag.tour_id = tourAgencyCost.TourId;
            return View(tourAgencyCost);
        }

        // GET: TourAgencyCosts/Delete/5
        public ActionResult Delete(long? id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourAgencyCost tourAgencyCost = db.TourAgencyCosts.Find(id);
            if (tourAgencyCost == null)
            {
                return HttpNotFound();
            }

            Tour tour = db.Tours.Find(tourAgencyCost.TourId);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            return View(tourAgencyCost);
        }

        // POST: TourAgencyCosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            TourAgencyCost tourAgencyCost = db.TourAgencyCosts.Find(id);

            Tour tour = db.Tours.Find(tourAgencyCost.TourId);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            } 

            db.TourAgencyCosts.Remove(tourAgencyCost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public JsonResult GetDiscount(FormCollection formCollection)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            long AgId = 0;

            if (GlobalUser.isAdmin == false)
            {
                var ag1 = db.Agencies.FirstOrDefault(r => r.UserId == GlobalUser.CurrentUser.Id);
                if (ag1 != null)
                {
                    AgId = ag1.Id;
                }
            }

          
            var PriceStr = formCollection["price"];

            decimal price = 0;

            try
            {
                price = decimal.Parse(PriceStr);
            }
            catch { }
             

            long id = 0;
            try
            {
                id = long.Parse(AgId.ToString());
            }
            catch { }

            var ag = db.Agencies.Find(id);

            var CheckDiscountModel = new CheckDiscountModel() { AgDiscount=0 , FullDiscount=0 , UserDiscount=0 };
            if(ag != null && GlobalUser.isAdmin == false)
            {
                try
                {
                    CheckDiscountModel.UserDiscount =Math.Round( (ag.UserDiscount * price)/100);
                    CheckDiscountModel.AgDiscount = Math.Round((ag.UserDiscount * price) / 100);
                    CheckDiscountModel.FullDiscount = CheckDiscountModel.UserDiscount+ CheckDiscountModel.AgDiscount;
                }
                catch { }
            }

            var output1 = JsonConvert.SerializeObject(CheckDiscountModel);
            return Json(output1, JsonRequestBehavior.AllowGet);
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
    public class CheckDiscountModel
    {
        public decimal UserDiscount { get; set; }
        public decimal AgDiscount { get; set; }
        public decimal FullDiscount { get; set; }
    }
}
