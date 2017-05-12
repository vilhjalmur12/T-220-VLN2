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
            ViewBag.newGoal = new GoalViewModel()
            {
                ProjectID = OpenProjectModel.ID,
                AspNetUserID = User.Identity.GetUserId()
            };
            //For the Editor
            List<FileViewModel> AllSolutionFiles = projectService.GetFilesByProject(projectID.Value);
            ViewBag.AllSolutionFiles = AllSolutionFiles;
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

        public void ChangeGoal (int goalID)
        {
            projectService.ChangeGoal(goalID);

        }

        public ActionResult RemoveMember(MembershipViewModel membership)
        {
            projectService.RemoveUserFromProject(membership);
            //LAGA
            return RedirectToAction("Index", "Project", new { projectID = membership.ProjectID, tapMake = "project-members"});
        }

        [HttpPost]
        public ActionResult NewGoal(GoalViewModel goal)
        {
            GoalViewModel newGoal = new GoalViewModel()
            {
                AspNetUserID = goal.AspNetUserID,
                name = goal.name,
                description = goal.description,
                ProjectID = goal.ProjectID,
                finished = false
            };

            projectService.AddNewGoal(newGoal);
            return RedirectToAction("Index", "Project", new { projectID = goal.ProjectID, tabMake = "project-goals" });
        }


        public ActionResult RemoveGoal(GoalViewModel goal)
        {
            projectService.RemoveGoal(goal);
            return RedirectToAction("ShowGoals", "Project");
        }

        public ActionResult NewObjective(int goalID, string name, int projectID)
        {
            ObjectiveViewModel newObjective = new ObjectiveViewModel()
            {
                name = name,
                GoalID = goalID,
                AspNetUserID = User.Identity.GetUserId(),
                finished = false
            };

            projectService.AddNewObjective(newObjective);
            return RedirectToAction("Index", "Project", new { projectID = projectID, tabMake = "project-goals" });
        }

        public ActionResult RemoveObjective(int objectiveID)
        {
            projectService.RemoveObjective(objectiveID);
            //LAGA
            return RedirectToAction("ShowGoals", "Project");
        }

        //public void SaveComment(string content)
        //{
        //    if (!String.IsNullOrEmpty(content))
        //    {
        //        CommentViewModel commentModel = new CommentViewModel()
        //        {
        //            AspNetUserID = User.Identity.GetUserId(),
        //            Content = content,
        //            ProjectID = OpenProjectModel.ID
        //        };

        //        projectService.AddNewComment(commentModel);
        //    }
        //}

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

        [HttpPost]
        public ActionResult OpenFile(string fileID)
        {
            int intFileID = Int32.Parse(fileID);
           FileViewModel NewDoc = projectService.GetFileByID(intFileID);
            string ext = NewDoc.FileType.Extension;
            Debug.WriteLine("Id int: " + intFileID);
            Debug.WriteLine("Document: " + NewDoc.name);
            Debug.WriteLine("Extension: " + ext);

            return Json(NewDoc, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult LeaveProject(MembershipViewModel membership)
        {
            membership.AspNetUserID = User.Identity.GetUserId();
            projectService.RemoveUserFromProject(membership);

            return RedirectToAction("Index", "UserHome");
        }


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

        public void SaveComment(int projectID, string message)
        {
            CommentViewModel newComment = new CommentViewModel()
            {
                ProjectID = projectID,
                Content = message,
                AspNetUserID = User.Identity.GetUserId(),
            };
            projectService.SaveComment(newComment);
            //return RedirectToAction("Index", "Project", new { projectID = projectID, tabMake = "project-chat" });
        }
    }
}