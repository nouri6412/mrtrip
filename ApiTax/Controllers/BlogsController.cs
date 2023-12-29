using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApiTax.Models;

namespace ApiTax.Controllers
{
    public class BlogsController : Controller
    {
        private MrTripEntities db = new MrTripEntities();

        // GET: Blogs
        public ActionResult Index()
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false ||  GlobalUser.isAdmin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }
            return View(db.Blogs.ToList());
        }

        // GET: Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false || GlobalUser.isAdmin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }
            ViewBag.BlogTagID = new SelectList(db.BlogTags, "BlogTagID", "TitleFa");
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogID,BlogTagID,TitleFa,TitleEn,ContentFa,ContentEn,Tag")] Blog blog)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false || GlobalUser.isAdmin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }

            if (ModelState.IsValid)
            {
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogTagID = new SelectList(db.BlogTags, "BlogTagID", "TitleFa",blog.BlogTagID);
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false || GlobalUser.isAdmin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogTagID = new SelectList(db.BlogTags, "BlogTagID", "TitleFa", blog.BlogTagID);
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogID,BlogTagID,TitleFa,TitleEn,ContentFa,ContentEn,Tag")] Blog blog)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false || GlobalUser.isAdmin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }
            if (ModelState.IsValid)
            {
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogTagID = new SelectList(db.BlogTags, "BlogTagID", "TitleFa", blog.BlogTagID);
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false || GlobalUser.isAdmin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if (GlobalUser.isLogin == false || GlobalUser.isAdmin == false)
            {
                return RedirectToAction("Login", "Home", new { });
            }

            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
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
