using CodeEditorApp.Models.ViewModels;
using CodeEditorApp.Repositories;
using CodeEditorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using CodeEditorApp.Models;

namespace CodeEditorApp.Controllers
{
    public enum ObjectType
    {
        Folder,
        Project,
        File
    }
    public class UserHomeController : Controller
    {
        private UserHomeRepository UserHome = new UserHomeRepository();
        // GET: UserHome
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            // List<Project> model = UserHome.GetAllProjects(userId);
            // return View(model);
            return View();
        }

        public ActionResult CreateProject()
        {
            //TODO
            return null;
        }

        public ActionResult OpenProject(int projectID)
        {
            //TODO
            return null;
        }

        public ActionResult OpenProjectByFile(int fileID)
        {
            //TODO
            return null;
        }

        public ActionResult CreateFolder()
        {
            //TODO
            return null;
        }

        public ActionResult DeleteType(ObjectType type, int ID)
        {
            //TODO
            return null;
        }

        public ActionResult MovePath(ObjectType type, string newPath)
        {
            //TODO
            return null;
        }
    }
}