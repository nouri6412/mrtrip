﻿using ApiTax.Models;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Content;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using ApiTax.Entities;

namespace ApiTax.Controllers
{
    public class HomeController : Controller
    {
   
        public ActionResult Index()
        {
            InitRequest InitRequest = new InitRequest();
            InitRequest.init(User);
            if(GlobalUser.isLogin == false)
            {
                return RedirectToAction("Login","Home",new { });
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User userr)
        {
            //if (ModelState.IsValid)
            //{

            func func = new func();
            if (func.IsValid(userr.Phone, userr.Password))
            {               
                FormsAuthentication.SetAuthCookie(userr.Phone, true);
     
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "اطلاعات ورود اشتباه است");
            }
            return View(userr);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}