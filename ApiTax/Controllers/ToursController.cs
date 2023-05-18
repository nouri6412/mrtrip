using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApiTax.Models;
using PagedList;

namespace ApiTax.Controllers
{
    public class ToursController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: Tours
        public ActionResult Index(int? page,string search="")
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            int pageSize = 15;
            int pageIndex = 1;

            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            if (GlobalUser.isLogin==false)
            {
                return RedirectToAction("Login","Home",new { });
            }
            ViewBag.page_index = pageIndex;
            if (GlobalUser.isAdmin)
            {
                var tours = db.Tours.Include(t => t.Hardness).Include(t => t.LocCity).Include(t => t.TourType).Include(t => t.TransportType).Include(t => t.User).Include(t => t.User1);
                return View(tours.OrderByDescending(r => r.Id).ToPagedList(pageIndex, pageSize));
            }
            else
            {
                var tours = db.Tours.Where(r=>r.Deleted!=true && r.UserId == GlobalUser.CurrentUser.Id).Include(t => t.Hardness).Include(t => t.LocCity).Include(t => t.TourType).Include(t => t.TransportType).Include(t => t.User).Include(t => t.User1);
                return View(tours.OrderByDescending(r => r.Id).ToPagedList(pageIndex, pageSize));
            }
        }

        // GET: Tours/Details/5
        public ActionResult Details(long? id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Tour tour = db.Tours.Find(id);

            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // GET: Tours/Create
        public ActionResult Create()
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }

            ViewBag.HardnessId = new SelectList(db.Hardnesses, "Id", "Title");
            ViewBag.FromCityId = new SelectList(db.LocCities, "Id", "Title");
            ViewBag.TourTypeId = new SelectList(db.TourTypes, "Id", "Title");
            ViewBag.TransportTypeId = new SelectList(db.TransportTypes, "Id", "Title");
            ViewBag.EquipmentsId = new SelectList(db.Equipments.OrderBy(r=>r.Title), "Id", "Title");

            ViewBag.TourEquipmentValue = new List<TourEquipment>();

            var br = from br1 in db.Users.Where(r=>r.UserType.Id==3)
                     select new { Id = br1.Id, Phone = br1.FirstName + " " + br1.LastName };

            ViewBag.SupervisorId = new SelectList(br, "Id", "Phone");
            ViewBag.ErrorMessage = "";
            Tour tour = new Tour() { IsActive=true};
            return View(tour);
        }

        // POST: Tours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,TourTypeId,HardnessId,FromCityId,TransportTypeId,HasHotel,Description,Nights,StartDate,EndDate,IsActive,ImageUrl,Canceled,Finished,StartPlace,SupervisorId,SupervisorName,Services,PhoneNumber,Capacity,Budget,CreateDate,Deleted,DeleteDate")] Tour tour)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }

      

            try
            {
                var date = Request.Form["StartDatePersian"];
                tour.CreateDate= utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }

            try
            {
                var date = Request.Form["EndDatePersian"];
                tour.EndDate = utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }

            ViewBag.HardnessId = new SelectList(db.Hardnesses, "Id", "Title", tour.HardnessId);
            ViewBag.FromCityId = new SelectList(db.LocCities, "Id", "Title", tour.FromCityId);
            ViewBag.TourTypeId = new SelectList(db.TourTypes, "Id", "Title", tour.TourTypeId);
            ViewBag.TransportTypeId = new SelectList(db.TransportTypes, "Id", "Title", tour.TransportTypeId);
            ViewBag.EquipmentsId = new SelectList(db.Equipments.OrderBy(r => r.Title), "Id", "Title");


            var br = from br1 in db.Users.Where(r => r.UserType.Id == 3)
                     select new { Id = br1.Id, Phone = br1.FirstName + " " + br1.LastName };

            ViewBag.SupervisorId = new SelectList(br, "Id", "Phone", tour.SupervisorId);

            tour.UserId = GlobalUser.CurrentUser.Id;

            var TourEquipmentList = new List<TourEquipment>();
            var TourEquipmentValue = Request.Form["TourEquipmentValue"];

            string[] sp = new string[10];

            try
            {
                if (TourEquipmentValue != null)
                {
                    sp = TourEquipmentValue.ToString().Split(',');
                }
                else
                {
                    sp = new string[0];
                }
            }
            catch
            {
                sp = new string[0];
            }

            try
            {
                foreach (var it in sp)
                {
                    int id = int.Parse(it);
                    var eq = new TourEquipment()
                    {
                        EquipmentId = id
                    };
                    TourEquipmentList.Add(eq);
                }
            }
            catch { }

            ViewBag.TourEquipmentValue = TourEquipmentList;

            HttpPostedFileBase file = Request.Files["Image"];
            if (file != null && file.ContentLength > 0)
                try
                {
                    if(file.ContentLength > 110000)
                    {
                        ViewBag.ErrorMessage = "حجم تصویر نباید بیشتر از 100 کیلوبایت باشد";
                      
                        return View(tour);
                    }
                    string new_name = Guid.NewGuid() + "-" + file.FileName;
                    string path = Path.Combine(Server.MapPath("~/Uploads"),
                                               Path.GetFileName(new_name));
                    file.SaveAs(path);
                    tour.ImageUrl = new_name;
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                
                    ViewBag.ErrorMessage = "ERROR:" + ex.Message.ToString();
                    return View(tour);
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }




            Boolean added = false;

            if (ModelState.IsValid)
            {
                db.Tours.Add(tour);
                db.SaveChanges();
                added = true;

            }

            if(added)
            {
                try
                {
                    dbEntities db1 = new dbEntities();
                    foreach (var it in sp)
                    {
                        int id = int.Parse(it);
                        var eq = new TourEquipment()
                        {
                            TourId = tour.Id,
                            EquipmentId = id
                        };
                        db1.TourEquipments.Add(eq);
                        db1.SaveChanges();

                    }
                    db1.Dispose();

                    return RedirectToAction("Edit", "Tours", new { id = tour.Id, type = "day" });

                }
                catch (Exception ex)
                {

                }
            }
            ViewBag.ErrorMessage = "به خطاها توجه نمائید";
     
            return View(tour);
        }

        // GET: Tours/Edit/5
        public ActionResult Edit(long? id,string type="")
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);

            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }


            ViewBag.HardnessId = new SelectList(db.Hardnesses, "Id", "Title", tour.HardnessId);
            ViewBag.FromCityId = new SelectList(db.LocCities, "Id", "Title", tour.FromCityId);
            ViewBag.TourTypeId = new SelectList(db.TourTypes, "Id", "Title", tour.TourTypeId);
            ViewBag.TransportTypeId = new SelectList(db.TransportTypes, "Id", "Title", tour.TransportTypeId);
            ViewBag.EquipmentsId = new SelectList(db.Equipments.OrderBy(r => r.Title), "Id", "Title");
            ViewBag.TourEquipmentValue = db.TourEquipments.Where(r=>r.TourId==tour.Id).ToList();
            ViewBag.ErrorMessage = "";
            var br = from br1 in db.Users.Where(r => r.UserType.Id == 3)
                     select new { Id = br1.Id, Phone = br1.FirstName + " " + br1.LastName };

            ViewBag.SupervisorId = new SelectList(br, "Id", "Phone", tour.SupervisorId);
            ViewBag.type = type;
            return View(tour);
        }

        // POST: Tours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,TourTypeId,HardnessId,FromCityId,TransportTypeId,HasHotel,Description,Nights,StartDate,EndDate,IsActive,ImageUrl,Canceled,Finished,StartPlace,SupervisorId,SupervisorName,Services,PhoneNumber,Capacity,Budget,CreateDate,Deleted,DeleteDate")] Tour tour)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }
            dbEntities db1 = new dbEntities();

            Tour tour1 = db1.Tours.Find(tour.Id);
            if (tour1 == null || (tour1.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            var TourEquipmentList = new List<TourEquipment>();
            var TourEquipmentValue = Request.Form["TourEquipmentValue"];

            string[] sp = new string[10];

            try
            {
                if (TourEquipmentValue != null)
                {
                    sp = TourEquipmentValue.ToString().Split(',');
                }
                else
                {
                    sp = new string[0];
                }
            }
            catch
            {
                sp = new string[0];
            }

            try
            {
                foreach (var it in sp)
                {
                    int id = int.Parse(it);
                    var eq = new TourEquipment()
                    {
                        EquipmentId = id
                    };
                    TourEquipmentList.Add(eq);
                }
            }
            catch { }

            ViewBag.TourEquipmentValue = TourEquipmentList;

            try
            {
                var date = Request.Form["StartDatePersian"];
                tour.CreateDate = utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }

            try
            {
                var date = Request.Form["EndDatePersian"];
                tour.EndDate = utility.ToMiladi(utility.toEnglishNumber(date.ToString()));
            }
            catch { }

            ViewBag.HardnessId = new SelectList(db.Hardnesses, "Id", "Title", tour.HardnessId);
            ViewBag.FromCityId = new SelectList(db.LocCities, "Id", "Title", tour.FromCityId);
            ViewBag.TourTypeId = new SelectList(db.TourTypes, "Id", "Title", tour.TourTypeId);
            ViewBag.TransportTypeId = new SelectList(db.TransportTypes, "Id", "Title", tour.TransportTypeId);
            ViewBag.EquipmentsId = new SelectList(db.Equipments.OrderBy(r => r.Title), "Id", "Title");


            var br = from br1 in db.Users.Where(r => r.UserType.Id == 3)
                     select new { Id = br1.Id, Phone = br1.FirstName + " " + br1.LastName };

            ViewBag.SupervisorId = new SelectList(br, "Id", "Phone", tour.SupervisorId);
            ViewBag.type = "";

            tour.ImageUrl = tour1.ImageUrl;
            HttpPostedFileBase file = Request.Files["Image"];
            if (file != null && file.ContentLength > 0)
                try
                {
                    if (file.ContentLength > 110000)
                    {
                        db1.Dispose();
                        ViewBag.ErrorMessage = "حجم تصویر نباید بیشتر از 100 کیلوبایت باشد";
                        return View(tour);
                    }
                    string new_name = Guid.NewGuid() + "-" + file.FileName;
                    string path = Path.Combine(Server.MapPath("~/Uploads"),
                                               Path.GetFileName(new_name));
                    file.SaveAs(path);
                    tour.ImageUrl = new_name;
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    db1.Dispose();
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    return View(tour);
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            tour.UserId = tour1.UserId;

            Boolean edited = false;
            if (ModelState.IsValid)
            {
                db.Entry(tour).State = EntityState.Modified;
       //         UpdateModel(tour);
                db.SaveChanges();
                edited = true;

            }

            if(edited)
            {
                try
                {


                    var rts = db1.TourEquipments.Where(r => r.TourId == tour.Id).ToList();

                    foreach (var item in rts)
                    {
                        var t1 = db1.TourEquipments.Find(item.Id);
                        db1.TourEquipments.Remove(t1);
                        db1.SaveChanges();
                    }

                    foreach (var it in sp)
                    {
                        int id = int.Parse(it);
                        var eq = new TourEquipment()
                        {
                            TourId = tour.Id,
                            EquipmentId = id
                        };
                        db1.TourEquipments.Add(eq);
                        db1.SaveChanges();

                    }

                    db1.Dispose();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                }
            }


            ViewBag.ErrorMessage = "به خطاها توجه نمائید";

            db1.Dispose();
            return View(tour);
        }

        

        // GET: Tours/Delete/5
        public ActionResult Delete(long? id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }
            return View(tour);
        }


        public ActionResult SetCheck(long TourId,string prop,int page=1)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }

            Tour tour = db.Tours.Find(TourId);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }
            if(prop== "HasHotel")
            {
                tour.HasHotel = !tour.HasHotel;
            }
            else if (prop == "IsActive")
            {
                tour.IsActive = !tour.IsActive;
            }
            else if (prop == "Canceled")
            {
                tour.Canceled = !tour.Canceled;
            }
            else if (prop == "Finished")
            {
                tour.Finished = !tour.Finished;
            }

            db.Entry(tour).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index",new { page=page});
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }

            Tour tour = db.Tours.Find(id);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            if(GlobalUser.isAdmin)
            {
                db.Tours.Remove(tour);
                db.SaveChanges();
            }
            else
            {
                tour.Deleted = true;
                tour.DeleteDate = DateTime.Now;
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
            }
          
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
