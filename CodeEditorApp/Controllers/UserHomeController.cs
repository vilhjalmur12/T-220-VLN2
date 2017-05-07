using CodeEditorApp.Models.ViewModels;
using CodeEditorApp.Repositories;
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
            string UserId = User.Identity.GetUserId();
            ViewBag.UserId = UserId;
            // List<ProjectViewModel> model = UserHome.GetAllProjects(UserId);
            // return View(model);
            return View();
        }

        [HttpPost]
        public ActionResult CreateProject(ProjectViewModel model)
        {
            Project NewProject = new Project();
            NewProject.ID = model.ID;
            NewProject.name = model.name;
            NewProject.AspNetUserID = User.Identity.GetUserId();
            NewProject.ProjectTypeID = model.TypeID;

            UserHome.CreateProject(NewProject);


            return RedirectToAction("Index");
        }

        public ActionResult OpenProject(int projectID)
        {
            //TODO

            return null;
        }

        public ActionResult OpenProjectByFile(int fileID)
        {
            FileViewModel file = UserHome.GetFile(fileID);
            //TOD GetProject
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



        /// <summary>
        /// Finds current users UserID, and looks for all information about that user.
        /// ApplicationUser holds all user info.
        /// </summary>
        /// <returns>A single ApplicationUser</returns>
        public UserViewModel GetUserInfo ()
        {
            string UserID = User.Identity.GetUserId();
            UserViewModel ReturnUser = UserHome.GetUser(UserID);
            return ReturnUser;
        }
    }
}