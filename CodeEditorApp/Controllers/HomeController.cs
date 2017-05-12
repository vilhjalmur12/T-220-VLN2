using System.Web.Mvc;

namespace CodeEditorApp.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Start page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// About page
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "About the ColabCode Project.";

            return View();
        }

        /// <summary>
        /// Terms page
        /// </summary>
        /// <returns></returns>
        public ActionResult Terms()
        {
            ViewBag.Message = "Terms and conditions.";

            return View();
        }

        /// <summary>
        /// Team page
        /// </summary>
        /// <returns></returns>
        public ActionResult Team()
        {
            ViewBag.Message = "A great text about our team!";
            return View();
        }

        /// <summary>
        /// Help page
        /// </summary>
        /// <returns></returns>
        public ActionResult Help()
        {
            ViewBag.Message = "Helping a friend.";
            return View();
        }
    }
}