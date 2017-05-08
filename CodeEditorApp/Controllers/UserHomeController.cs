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
using System.Diagnostics;

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
            string userID = User.Identity.GetUserId();

            UserViewModel model = new UserViewModel()
            {
                ID = userID,
                UserName = User.Identity.GetUserName(),
                Projects = UserHome.GetAllProjects(userID)
            };

            ViewBag.Root = GetFileTree(userID);
            Debug.WriteLine("username"+User.Identity.GetUserId());
            Debug.WriteLine(UserHome.GetAllProjects(User.Identity.GetUserId()).Count());

            return View(model);
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

        public ActionResult OpenProject(int? projectID)
        {
            if (projectID.HasValue)
            {
                ProjectViewModel model = UserHome.GetProjectByID(projectID.Value);

                return RedirectToAction("Index", "Project", model);
            }
            return RedirectToAction("Index", "Project");
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

        public RootFolderViewModel GetFileTree (string UserID)
        {
            return UserHome.GetUserRootFolder(UserID);
        }
    }
}