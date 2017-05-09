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
        private UserHomeRepository UserHome;

        public UserHomeController ()
        {
            UserHome = new UserHomeRepository();
        }

        // GET: UserHome
        public ActionResult Index()
        {

            ViewBag.NewProject = NewProject();

            return View(GetUserData());
        }

        public ProjectViewModel NewProject()
        {
            ProjectViewModel NewModel = new ProjectViewModel();

            NewModel.OwnerID = User.Identity.GetUserId();
            NewModel.AvailableProjects = GetAvailableProjectTypes();
            return NewModel;
        }



       [HttpGet]
        public ActionResult CreateProject()
        {
            ProjectViewModel NewModel = new ProjectViewModel();

            NewModel.OwnerID = User.Identity.GetUserId();
            NewModel.AvailableProjects = GetAvailableProjectTypes();

            return View(NewModel);
        }

        [HttpPost]
        public ActionResult CreateProject(ProjectViewModel model)
        {
            Project NewProject = new Project();
  
            NewProject.name = model.name;
            NewProject.AspNetUserID = User.Identity.GetUserId();
            NewProject.ProjectTypeID = model.TypeID;
            
            UserHome.CreateProject(ref NewProject);

            model.ID = NewProject.ID;

            return OpenProject(NewProject.ID);
        }

        public ActionResult OpenProject(int? projectID)
        {
            if (projectID.HasValue)
            {
                Debug.WriteLine("Projecid");
                Debug.WriteLine(projectID.Value);
                TempData["projectModel"] = UserHome.GetProjectByID(projectID.Value);
                Debug.WriteLine("FJÖLDI SKR'AA");
                Debug.WriteLine(UserHome.GetProjectByID(projectID.Value).SolutionFolder.Files.Count());
                return RedirectToAction("Index", "Project");
            }

            return RedirectToAction("Index", "UserHome");
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
            string UserID = User.Identity.GetUserId();
            UserViewModel ReturnUser = UserHome.GetUser(UserID);
            return ReturnUser;
        }

        public RootFolderViewModel GetFileTree (string UserID)
        {
            return UserHome.GetUserRootFolder(UserID);
        }
        
        
        public List<SelectListItem> GetAvailableProjectTypes()
        {
            return UserHome.GetProjectTypes();
        }

        public List<SelectListItem> GetAvailableFileTypes()
        {
            return UserHome.GetFileTypes();
        }

        public ActionResult ClearUserData()
        {
            UserHome.ClearUserData(User.Identity.GetUserId());

            return RedirectToAction("Index", "UserHome");
        }

        public UserViewModel GetUserData ()
        {
            UserViewModel user = new UserViewModel();
            string userID = User.Identity.GetUserId();
            user.ID = userID;
            user.UserName = User.Identity.GetUserName();
            user.Projects = UserHome.GetAllProjects(userID);
            user.RootFolder = GetFileTree(userID);

            return user;
        }
        
    }
}