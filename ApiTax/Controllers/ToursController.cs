using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApiTax.Entities;
using ApiTax.Models;
using PagedList;

namespace ApiTax.Controllers
{
    public class ToursController : Controller
    {
        //private dbEntities db = new dbEntities();
        private readonly MrTripEntities db = new MrTripEntities();

        // GET: Tours
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
            if (GlobalUser.isAdmin)
            {
                var tours = db.Tours.Include(t => t.Hardness).Include(t => t.LocCity).Include(t => t.TourType).Include(t => t.TransportType).Include(t => t.User).Include(t => t.User1);
                return View(tours.OrderByDescending(r => r.Id).ToPagedList(pageIndex, pageSize));
            }
            else
            {
                var tours = db.Tours.Where(r => r.Deleted != true && r.UserId == GlobalUser.CurrentUser.Id).Include(t => t.Hardness).Include(t => t.LocCity).Include(t => t.TourType).Include(t => t.TransportType).Include(t => t.User).Include(t => t.User1);
                return View(tours.OrderByDescending(r => r.Id).ToPagedList(pageIndex, pageSize));
            }
        }
        public ActionResult IndexTrips(int? tour_id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);

            Tour tour = db.Tours.Find(tour_id);
            if (tour == null || (tour.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            var tours = db.TourBookingTrippers.Where(r=>r.TourBooking.IsPaid==true && r.TourBooking.TourId== tour_id);
             return View(tours.ToList());
           
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
            ViewBag.EquipmentsId = new SelectList(db.Equipments.OrderBy(r => r.Title), "Id", "Title");

            ViewBag.TourEquipmentValue = new List<Equipment>();

            var br = from br1 in db.Users.Where(r => r.UserType.Id == 3)
                     select new { Id = br1.Id, Phone = br1.FirstName + " " + br1.LastName };

            ViewBag.SupervisorId = new SelectList(br, "Id", "Phone");
            ViewBag.ErrorMessage = "";
            Tour tour = new Tour() { IsActive = true };
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

            tour.UserId = GlobalUser.CurrentUser.Id;

            var TourEquipmentList = new List<Equipment>();
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
                    var eq = new Equipment()
                    {
                        Id = id
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
                    if (file.ContentLength > 110000)
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

            if (added)
            {
                try
                {
                    MrTripEntities db1 = new MrTripEntities();
                    foreach (var it in sp)
                    {
                        int id = int.Parse(it);
                        //var eq = new TourEquipment()
                        //{
                        //    TourId = tour.Id,
                        //    EquipmentId = id
                        //};
                        var eq = db1.Equipments.Find(id);
                        tour.Equipments.Add(eq);
                    }
                    db1.SaveChanges();
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
        public ActionResult Edit(long? id, string type = "")
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
            ViewBag.EquipmentsId = new MultiSelectList(db.Equipments.OrderBy(r => r.Title), "Id", "Title");
            ViewBag.TourEquipmentValue = tour.Equipments.Select(x => x.Id).ToList();
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
            MrTripEntities db1 = new MrTripEntities();

            Tour tour1 = db1.Tours.Find(tour.Id);
            if (tour1 == null || (tour1.UserId != GlobalUser.CurrentUser.Id && GlobalUser.isAdmin == false))
            {
                return HttpNotFound();
            }

            var TourEquipmentList = new List<Equipment>();
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
                    var eq = new Equipment()
                    {
                        Id = id
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

            if (edited)
            {
                try
                {


                    var rts = tour.Equipments.ToList();

                    foreach (var item in rts)
                    {
                        var t1 = db1.Equipments.Find(item.Id);
                        tour1.Equipments.Remove(t1);
                    }
                    //db1.SaveChanges();

                    foreach (var it in sp)
                    {
                        int id = int.Parse(it);
                        var eq = db1.Equipments.Find(id);
                        tour1.Equipments.Add(eq);
                        db1.Entry(tour1).State = EntityState.Modified;
                    }
                    db1.SaveChanges();
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


        public ActionResult SetCheck(long TourId, string prop, int page = 1)
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
            if (prop == "HasHotel")
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

            return RedirectToAction("Index", new { page = page });
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

            if (GlobalUser.isAdmin)
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


        // GET: Tours/CreatePlus/5
        [Authorize]
        public ActionResult CreatePlus()
        {
            ViewBag.TourTypeId = new SelectList(db.TourTypes, "Id", "Title");
            ViewBag.HardnessId = new SelectList(db.Hardnesses, "Id", "Title");
            ViewBag.FromCityId = new SelectList(db.LocCities, "Id", "Title");
            ViewBag.ToCityId = new SelectList(db.LocCities, "Id", "Title");
            ViewBag.TransportTypeId = new SelectList(db.TransportTypes, "Id", "Title");
            ViewBag.Equipments = new SelectList(db.Equipments.OrderBy(r => r.Title), "Id", "Title");

            ViewBag.LocationId = new SelectList(db.Locations.Where(x => x.LocationType.GroupId == 1).OrderBy(r => r.Title), "Id", "Title");
            ViewBag.TransportCompanyId = new SelectList(db.TransportCompanies, "Id", "Title");
            ViewBag.DepartureStationId = new SelectList(db.Stations, "Id", "Title");
            ViewBag.ArrivalStationId = new SelectList(db.Stations, "Id", "Title");
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Title");

            ViewBag.AgencyId = new SelectList(db.Agencies, "Id", "Name");
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "Title");
            ViewBag.RoomTypeId = new SelectList(db.HotelRoomTypes, "Id", "Title");

            var supervisors = db.Users.Where(r => r.UserType.Id == 3)
                .Select(b => new { Id = b.Id, Name = b.FirstName + " " + b.LastName });

            ViewBag.SupervisorId = new SelectList(supervisors, "Id", "Name");

            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePlus(TourModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tour = new Tour
                    {
                        Title = model.Title,
                        TourTypeId = model.TourTypeId,
                        FromCityId = model.FromCityId,
                        TransportTypeId = model.TransportTypeId,
                        Nights = model.Nights,
                        StartDate = utility.ToMiladi(utility.toEnglishNumber(model.StartDateFa)),
                        EndDate = utility.ToMiladi(utility.toEnglishNumber(model.EndDateFa)),
                        Capacity = model.Capacity,
                        HardnessId = model.HardnessId,
                        StartPlace = model.StartPlace,
                        SupervisorId = model.SupervisorId,
                        PhoneNumber = model.PhoneNumber,
                        Budget = model.Budget,
                        ImageUrl = model.ImageUrl,
                        Services = model.Services,
                        Description = model.Description,
                        IsActive = true,
                        CreateDate = DateTime.Now,
                        HasHotel = model.HasHotel,
                        UserId = GlobalUser.CurrentUser.Id
                    };

                    foreach (var item in model.Equipments)
                    {
                        var eq = await db.Equipments.FindAsync(item);
                        tour.Equipments.Add(eq);
                    }

                    var firstStop = new TourStop
                    {
                        TransportCompanyId = model.TransportCompanyId,
                        DepartureDate = utility.ToMiladi(utility.toEnglishNumber(model.DepartureDateFa)),
                        DepartureDateFa = model.DepartureDateFa,
                        DepartureTime = model.DepartureTime,
                        DepartureStationId = model.DepartureStationId,
                        Duration = model.Duration,
                        WaitDuration = model.WaitDuration,
                        ArrivalTime = model.ArrivalTime,
                        ArrivalStationId = model.ArrivalStationId,
                        CityId = model.FromCityId,
                        StopTypeId = (int)StopType.Start,
                        StopOrder = 1,
                    };
                    tour.TourStops.Add(firstStop);

                    var hotelStop = new TourStop
                    {
                        DepartureDate = utility.ToMiladi(utility.toEnglishNumber(model.DepartureDateFa)),
                        DepartureDateFa = model.DepartureDateFa,
                        DepartureTime = model.DepartureTime,
                        TransportCompanyId = model.TransportCompanyId,
                        CityId = model.ToCityId,
                        StopTypeId = (int)StopType.Stayin,
                        LocationId = model.LocationId,
                        Nights = model.HotelNights,
                        StopOrder = 2,
                    };

                    if (model.HasHotel)
                        hotelStop.HotelId = model.HotelId;
                    tour.TourStops.Add(hotelStop);


                    var lastStop = new TourStop
                    {
                        TransportCompanyId = model.TransportCompanyId,
                        DepartureDate = utility.ToMiladi(utility.toEnglishNumber(model.DepartureDateFa)),
                        DepartureDateFa = model.DepartureDateFa,
                        CityId = model.FromCityId,
                        StopTypeId = (int)StopType.End,
                        StopOrder = 10
                    };
                    tour.TourStops.Add(lastStop);

                    var cost = new TourAgencyCost
                    {
                        AgencyId = model.AgencyId,
                        RoomTypeId = model.RoomTypeId,
                        Price = model.Price,
                        PackageNumber = model.PackageNumber,
                        CurrencyPrice = model.CurrencyPrice,
                        CurrencyId = model.CurrencyId,
                    };
                    if (GlobalUser.isAdmin == false)
                    {
                        var ag = db.Agencies.FirstOrDefault(r => r.UserId == GlobalUser.CurrentUser.Id);
                        if (ag != null)
                        {
                            cost.AgencyId = ag.Id;
                            cost.UserDiscount = (ag.UserDiscount * cost.Price) / 100;
                            cost.AffiliateDiscount = (ag.UserDiscount * cost.Price) / 100;
                            cost.FullDiscount = cost.UserDiscount + cost.AffiliateDiscount;
                        }
                    }
                    tour.TourAgencyCosts.Add(cost);

                    if (Request.Files["Image"] != null && UploadImage(Request.Files["Image"], out string imageUrl, out string message))
                    {
                        ModelState.AddModelError("ImageUrl", message);
                    }
                    else
                    {
                        db.Tours.Add(tour);
                        await db.SaveChangesAsync();

                        return RedirectToAction("Edit", "Tours", new { id = tour.Id, type = "day" });
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                var message = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    message += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        message += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                ModelState.AddModelError("", message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.TourTypeId = new SelectList(db.TourTypes, "Id", "Title", model.TourTypeId);
            ViewBag.HardnessId = new SelectList(db.Hardnesses, "Id", "Title", model.HardnessId);
            ViewBag.FromCityId = new SelectList(db.LocCities, "Id", "Title", model.FromCityId);
            ViewBag.ToCityId = new SelectList(db.LocCities, "Id", "Title", model.ToCityId);
            ViewBag.TransportTypeId = new SelectList(db.TransportTypes, "Id", "Title", model.TransportTypeId);
            ViewBag.Equipments = new MultiSelectList(db.Equipments.OrderBy(r => r.Title), "Id", "Title", model.Equipments);

            ViewBag.LocationId = new SelectList(db.Locations.Where(x => x.LocationType.GroupId == 1).OrderBy(r => r.Title), "Id", "Title", model.LocationId);
            ViewBag.TransportCompanyId = new SelectList(db.TransportCompanies, "Id", "Title", model.TransportCompanyId);
            ViewBag.DepartureStationId = new SelectList(db.Stations, "Id", "Title", model.DepartureStationId);
            ViewBag.ArrivalStationId = new SelectList(db.Stations, "Id", "Title", model.ArrivalStationId);
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Title", model.HotelId);

            ViewBag.AgencyId = new SelectList(db.Agencies, "Id", "Name", model.AgencyId);
            ViewBag.CurrencyId = new SelectList(db.Currencies, "Id", "Title", model.CurrencyId);
            ViewBag.RoomTypeId = new SelectList(db.HotelRoomTypes, "Id", "Title", model.RoomTypeId);

            var supervisors = db.Users.Where(r => r.UserType.Id == 3)
                .Select(b => new { Id = b.Id, Name = b.FirstName + " " + b.LastName });

            ViewBag.SupervisorId = new SelectList(supervisors, "Id", "Name", model.SupervisorId);

            return View(model);
        }







        public bool UploadImage(HttpPostedFileBase file, out string imageUrl, out string message)
        {
            imageUrl = "";
            if (file != null && file.ContentLength > 0)
                try
                {
                    if (file.ContentLength > 110000)
                    {
                        message = "حجم تصویر نباید بیشتر از 100 کیلوبایت باشد";
                        return false;
                    }
                    string new_name = Guid.NewGuid() + "-" + file.FileName;
                    string path = Path.Combine(Server.MapPath("~/Uploads"),
                                               Path.GetFileName(new_name));
                    file.SaveAs(path);
                    imageUrl = new_name;
                    message = "File uploaded successfully";
                    return true;
                }
                catch (Exception ex)
                {
                    message = "ERROR:" + ex.Message.ToString();
                    return false;
                }
            else
            {
                message = "You have not specified a file.";
                return false;
            }
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
