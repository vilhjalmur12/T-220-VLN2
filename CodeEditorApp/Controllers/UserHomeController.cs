using CodeEditorApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeEditorApp.Controllers
{
    public class UserHomeController : Controller
    {
        private UserHomeRepository UserHome = new UserHomeRepository();
        // GET: UserHome
        public ActionResult Index()
        {
            return View();
        }
    }
}