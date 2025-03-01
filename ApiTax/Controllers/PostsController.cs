﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApiTax.Entities;
using ApiTax.Models;

namespace ApiTax.Controllers
{
    public class PostsController : Controller
    {
        private MrTripEntities db = new MrTripEntities();

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.PostCategory).Include(p => p.User);
            return View(posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.PostCategories, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Phone");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,TitleEn,Summary,SummaryEn,Body,BodyEn,CategoryId,Tags,ImageUrl,IsActive,CreateDate,UserId")] Post post)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);


            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.PostCategories, "Id", "Title", post.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Phone", post.UserId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.PostCategories, "Id", "Title", post.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Phone", post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,TitleEn,Summary,SummaryEn,Body,BodyEn,CategoryId,Tags,ImageUrl,IsActive,CreateDate,UserId")] Post post)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);


            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.PostCategories, "Id", "Title", post.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Phone", post.UserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);


            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);


            if (GlobalUser.isAdmin == false)
            {
                return HttpNotFound();
            }
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
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
