﻿using CodeEditorApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeEditorApp.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About this awesome page.";

            return View();
        }

        public ActionResult Terms()
        {
            ViewBag.Message = "Terms and conditions.";

            return View();
        }

        public ActionResult Team()
        {
            ViewBag.Message = "A great text about our team!";
            return View();
        }

        public ActionResult Help()
        {
            ViewBag.Message = "Helping a friend.";
            return View();
        }
    }
}