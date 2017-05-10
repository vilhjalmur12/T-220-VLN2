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
                AvailableProjects = UserHomeService.GetProjectTypes()
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

            // return OpenProject(newProject.ID);
            return RedirectToAction("Index", "Project", new { projectID = projectModel.ID });
        }


        public ActionResult CreateFolder()
        {
            //TODO
            return null;
        }

        public ActionResult DeleteFolder(int folderID)
        {
            UserHomeService.DeleteFolder(folderID);
            return null;
        }

        public ActionResult DeleteFile(int fileID)
        {
            UserHomeService.DeleteFile(fileID);
            return null;
        }

        public ActionResult DeleteProject(int projectID)
        {
            UserHomeService.DeleteProject(projectID);
            return null;
        }
  

        public ActionResult ClearUserData()
        {
            UserHomeService.ClearUserData(User.Identity.GetUserId());

            return RedirectToAction("Index", "UserHome");
        }

        private UserViewModel GetUserData()
        {
            string userID = User.Identity.GetUserId();

            UserViewModel user = new UserViewModel()
            {
                ID = userID,
                UserName = User.Identity.GetUserName(),
                Projects = UserHomeService.GetAllProjects(userID),
                RootFolder = UserHomeService.GetUserRootFolder(userID)
            };

            return user;
        }
        
    }
}