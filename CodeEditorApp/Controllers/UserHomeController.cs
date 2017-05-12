using CodeEditorApp.Models.ViewModels;
using CodeEditorApp.Repositories;
using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using CodeEditorApp.Models;
using System.Diagnostics;

namespace CodeEditorApp.Controllers
{
    public class UserHomeController : Controller
    {
        private UserHomeRepository UserHomeService;

        //---------Public Functions --------------
        public UserHomeController()
        {
            UserHomeService = new UserHomeRepository();
        }

        /// <summary>
        /// Returns the main page for user
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.NewProject = NewProjectModel();
            return View (GetUserData());
        }

        //---------Private Functions --------------
        /// <summary>
        /// Creates and returns a new projectViewModel
        /// Sets OwnerID to the current UserID 
        /// Gets available projectTypes
        /// </summary>
        /// <returns>ProjectViewModel</returns>
        private ProjectViewModel NewProjectModel()
        {
            ProjectViewModel newProject = new ProjectViewModel()
            {
                OwnerID = User.Identity.GetUserId(),
                AvailableProjects = UserHomeService.GetProjectTypes()
            };

            return newProject;
        }


        [HttpPost]
        public ActionResult CreateProject(ProjectViewModel projectModel)
        {
            if ((projectModel.Name != null) && (projectModel.Name.Length > 0))
            {
                if (projectModel.TypeID == 0)
                {
                    // Make empty project
                    projectModel.TypeID = 1;
                }
                Project newProject = new Project()
                {
                    name = projectModel.Name,
                    AspNetUserID = User.Identity.GetUserId(),
                    ProjectTypeID = projectModel.TypeID
                };

                UserHomeService.CreateProject(ref newProject);
                return RedirectToAction("Index", "Project", new { projectID = newProject.ID });
            }

            return RedirectToAction("Index", "UserHome");
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

        public ActionResult DeleteProject(int? projectID)
        {
            UserHomeService.DeleteProject(projectID.Value);
            return RedirectToAction("Index", "Userhome");
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

        public void DeleteFileJSON(string fileID)
        {
            Debug.WriteLine("DelteFile: " + fileID);
            int intFileID = Convert.ToInt32(fileID);

            if (intFileID != 0)
            {
                UserHomeService.RemoveFile(intFileID);
            }

        }

    }
}