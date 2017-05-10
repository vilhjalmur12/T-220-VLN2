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
        private UserHomeRepository UserHomeService;

        public UserHomeController()
        {
            UserHomeService = new UserHomeRepository();
        }

        // GET: UserHome
        public ActionResult Index()
        {

            ViewBag.NewProject = NewProject();

            return View (GetUserData());
        }

        public ProjectViewModel NewProject()
        {
            ProjectViewModel newProject = new ProjectViewModel()
            {
                OwnerID = User.Identity.GetUserId(),
                AvailableProjects = GetAvailableProjectTypes()
            };

            return newProject;
        }

       [HttpGet]
        public ActionResult CreateProject()
        {
            ProjectViewModel newProject = NewProject();
            return View(newProject);
        }

        [HttpPost]
        public ActionResult CreateProject(ProjectViewModel projectModel)
        {
            Project newProject = new Project()
            {
                name = projectModel.name,
                AspNetUserID = User.Identity.GetUserId(),
                ProjectTypeID = projectModel.TypeID
            };

            UserHomeService.CreateProject(ref newProject);
            projectModel.ID = newProject.ID;

            return OpenProject(newProject.ID);
        }

        public ActionResult OpenProject(int? projectID)
        {
            if (projectID.HasValue)
            {
                TempData["projectModel"] = UserHomeService.GetProjectByID(projectID.Value);
                return RedirectToAction("Index", "Project");
            }

            return RedirectToAction("Index", "UserHome");
        }

        public ActionResult OpenProjectByFile(int fileID)
        {
            FileViewModel file = UserHomeService.GetFile(fileID);
            //TOD GetProject
            return null;
        }

        public ActionResult CreateFolder()
        {
            //TODO
            return null;
        }

        [HttpGet]
        public ActionResult CreateFile()
        {
            FileViewModel model = new FileViewModel();
            model.AvailableTypes = GetAvailableFileTypes();
            
            

            return null;
        }

        [HttpPost]
        public ActionResult CreateFile(FileViewModel model)
        {

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
            string userID = User.Identity.GetUserId();
            UserViewModel returnUser = UserHomeService.GetUser(userID);
            return returnUser;
        }

        public RootFolderViewModel GetFileTree (string userID)
        {
            return UserHomeService.GetUserRootFolder(userID);
        }
        
        
        public List<SelectListItem> GetAvailableProjectTypes()
        {
            return UserHomeService.GetProjectTypes();
        }

        public List<SelectListItem> GetAvailableFileTypes()
        {
            return UserHomeService.GetFileTypes();
        }

        public ActionResult ClearUserData()
        {
            UserHomeService.ClearUserData(User.Identity.GetUserId());

            return RedirectToAction("Index", "UserHome");
        }

        public UserViewModel GetUserData ()
        {
            string userID = User.Identity.GetUserId();

            UserViewModel user = new UserViewModel()
            {
                ID = userID,
                UserName = User.Identity.GetUserName(),
                Projects = UserHomeService.GetAllProjects(userID),
                RootFolder = GetFileTree(userID)
            };

            return user;
        }
        
    }
}