using CodeEditorApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CodeEditorApp.Models.ViewModels;
using CodeEditorApp.Utils;

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
            ViewBag.Message = "About the ColabCode Project.";

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
        public ActionResult Editor()
        {
            int a = 0;
            int b = 10 / a;

            //ViewBag.Code = "alert('Hello World!');";
            //ViewBag.DocumentID = 17;
            return View();
        }
        public ActionResult SaveCode(EditorViewModel model)
        {
            return View("Home");
        }
    }
}