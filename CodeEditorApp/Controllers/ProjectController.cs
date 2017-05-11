using CodeEditorApp.Models;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeEditorApp.Repositories;
using Microsoft.AspNet.Identity;
using CodeEditorApp.Models.Entities;
using System.Data.Entity;
using System.Diagnostics;

namespace CodeEditorApp.Controllers
{
    public class ProjectController: Controller
    {

        private ProjectRepository projectService = new ProjectRepository();

        private OpenProjectViewModel OpenProjectModel;

        [HttpGet]
        public ActionResult Index(int? projectID, string tabMake)
        {
            if (tabMake == null)
            {
                tabMake = "";
            }

            OpenProjectModel = projectService.GetOpenProjectViewModel(projectID.Value);
            ViewBag.newFile = NewFile();
            ViewBag.newMembership = new MembershipViewModel()
            {
                ProjectID = OpenProjectModel.ID
            };
            //For the Editor
            ViewBag.Code = "alert('Hello World!');";
            ViewBag.DocumentID = 17;
            //ViewBag.ProjectID = projectID;
            ViewBag.UserName = User.Identity.GetUserName();
            ViewBag.UseID = User.Identity.GetUserId();
            //For the editor
            ViewBag.tabMake = tabMake;

            return View(OpenProjectModel);
        }

        private FileViewModel NewFile()
        {

            FileViewModel newFile = new FileViewModel()
            {
                ProjectID = OpenProjectModel.ID,
                AvailableTypes = projectService.GetFileTypes(),
                HeadFolderID = OpenProjectModel.SolutionFolder.ID
            };

            Debug.WriteLine(newFile.ProjectID);
            Debug.WriteLine(OpenProjectModel.ID);
            return newFile;
        }

        [HttpPost]
        public ActionResult ChangeGoal (int? goalID)
        {
            projectService.ChangeGoal(goalID.Value);
            // LAGA
            return RedirectToAction("Index", "Project", OpenProjectModel);
        }



        public ActionResult RemoveMember(MembershipViewModel membership)
        {
            projectService.RemoveUserFromProject(membership);
            //LAGA
            return RedirectToAction("Index", "Project", new { projectID = membership.ProjectID, tapMake = "project-members"});
        }

        public ActionResult AddGoal(FormCollection collection)
        {
            string goalName = collection["goalName"];
            string goalDescription = collection["goalDescription"];

            if (String.IsNullOrEmpty(goalName))
            {
                return View("Error");
            }
            if (String.IsNullOrEmpty(goalDescription))
            {
                return RedirectToAction("Index", "Project", new { id = OpenProjectModel.ID });
            }

            GoalViewModel newGoal = new GoalViewModel()
            {
                name = goalName,
                description = goalDescription,
                ProjectID = OpenProjectModel.ID,
                AspNetUserID = User.Identity.GetUserId(),
                finished = false
            };

            projectService.AddNewGoal(newGoal);
            //LAGA
            return RedirectToAction("ShowGoals", "project");
        }


        public ActionResult RemoveGoal(GoalViewModel goal)
        {
            projectService.RemoveGoal(goal);
            //LAGA
            return RedirectToAction("ShowGoals", "Project");
        }

        public ActionResult AddObjective(int goalID, FormCollection collection)
        {
            string objectiveName = collection["objectiveName"];

            if (String.IsNullOrEmpty(objectiveName))
            {
                return View("Error");
            }

            ObjectiveViewModel thisObjective = new ObjectiveViewModel()
            {
                name = objectiveName,
                GoalID = goalID,
                AspNetUserID = User.Identity.GetUserId(),
                finished = false
            };

            projectService.AddNewObjective(thisObjective);
            //LAGA
            return RedirectToAction("ShowGoals", "Project");
        }

        public ActionResult RemoveObjective(int objectiveID)
        {
            projectService.RemoveObjective(objectiveID);
            //LAGA
            return RedirectToAction("ShowGoals", "Project");
        }

        public void SaveComment(string content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                CommentViewModel commentModel = new CommentViewModel()
                {
                    AspNetUserID = User.Identity.GetUserId(),
                    Content = content,
                    ProjectID = OpenProjectModel.ID
                };

                projectService.AddNewComment(commentModel);
            }
        }

        [HttpGet]
        public ActionResult CreateFile()
        {
            FileViewModel newFile = NewFile();
            return View(newFile);
        }

        [HttpPost]
        public ActionResult CreateFile(FileViewModel fileModel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                File newFile = new File()
                {
                    name = fileModel.name,
                    FileType = projectService.GetFileTypeByID(fileModel.FileTypeID),
                    ProjectID = fileModel.ProjectID,
                    HeadFolderID = fileModel.HeadFolderID
                };

                projectService.CreateFile(ref newFile);

                //   return OpenFile(newFile.ID); eftir að útfæra

                return RedirectToAction("Index", "Project", new { projectID = newFile.ProjectID });
                
            } else
            {
                fileModel = NewFile();
                return View(fileModel);
            }  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(FileViewModel model, HttpPostedFileBase upload)
        {
            File fileUpload = new File();

                fileUpload.name = System.IO.Path.GetFileName(upload.FileName);
                fileUpload.FileType = projectService.GetFileTypeByExtension(System.IO.Path.GetExtension(upload.FileName));
                fileUpload.ProjectID = model.ProjectID;
                fileUpload.HeadFolderID = model.HeadFolderID;

                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    fileUpload.Content = reader.ReadString();
                }
            
            projectService.CreateFile(ref fileUpload);


            return RedirectToAction("Index", "Project", new { projectID = fileUpload.ProjectID });
        } 

        public ActionResult OpenFile(int? fileID)
        {
            //TODO
            return null;
        }

        public ActionResult CreateFolder()
        {
            //TODO
            return null;
        }

        public ActionResult CopyFile(int fileID)
        {
            //TODO
            return null;
        }

        public ActionResult PasteFile(int fileID)
        {
            //TODO
            return null;
        }

        public ActionResult DeleteFile(int fileID)
        {
            projectService.RemoveFile(fileID);
            //LAGA
            return RedirectToAction("Index", "Project", new { id = OpenProjectModel.ID });
        }

       /* public ActionResult LeaveProject()
        {
            projectService.RemoveUserFromProject(User.Identity.GetUserId(), OpenProjectModel.ID);
            return RedirectToAction("Index", "UserHome");
        }*/

        public ActionResult ChangeEditorColor()
        {
            //TODO
            return null;
        }
 
  /*      [HttpPost] // can be HttpGet
        public ActionResult AddMemberIfExists(string email, int projectID)
        {
            //if true (breyti repo fallinu úr add)
            //
            bool isValid = projectService.AddMemberIfExists(email, projectID); //.. check
            var obj = new
            {
                valid = isValid
            };

            return Json(obj);
        }*/

        /*[HttpPost] // can be HttpGet
        public ActionResult RemoveMemberIfInProject(string email, int projectID)
        {
            bool isValid = projectService.RemoveMemberIfInProject(email, projectID);
            var obj = isValid;

            return Json(new { valid = isValid });
        }*/

        [HttpPost]
        public ActionResult AddMember(MembershipViewModel membership)
        {
            projectService.AddMemberIfExists(membership);

            return RedirectToAction("Index", "Project", new { projectID = membership.ProjectID, tabMake = "project-members" });
        }

        public ActionResult SaveComment(int projectID, string message)
        {
            CommentViewModel newComment = new CommentViewModel()
            {
                ProjectID = projectID,
                Content = message,
                AspNetUserID = User.Identity.GetUserId(),
            };
            projectService.SaveComment(newComment);
            return RedirectToAction("Index", "Project", new { projectID = projectID, tabMake = "project-chat" });
        }
    }
}