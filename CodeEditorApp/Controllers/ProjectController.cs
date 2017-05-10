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
        public ActionResult Index(int? projectID)
        {
            OpenProjectModel = projectService.GetOpenProjectViewModel(projectID.Value);
            ViewBag.newFile = NewFile();
            //For the Editor
            ViewBag.Code = "alert('Hello World!');";
            ViewBag.DocumentID = 17;
            ViewBag.UserName = User.Identity.GetUserName();
            ViewBag.UseID = User.Identity.GetUserId();
            //For the editor

            return View(OpenProjectModel);
        }

        public FileViewModel NewFile()
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
            updateGoals();
            return RedirectToAction("Index", "Project", OpenProjectModel);
        }

        private void updateGoals()
        {
            OpenProjectModel.Goals = projectService.GetGoalsByProject(OpenProjectModel.ID);
        }

        private void updateComments()
        {
            OpenProjectModel.Comments = projectService.GetCommentsByProject(OpenProjectModel.ID);
        }

        private void updateUsers()
        {
            OpenProjectModel.Members = projectService.GetUsersByProject(OpenProjectModel.ID);
        }

        private void updateFiles()
        {
          //  projectModel.Files = project.GetFilesByProject(projectModel.ID);
        }

        private void updateFolders()
        {
          //  projectModel.Folders = project.GetFoldersByProject(projectModel.ID);
        }

        public ActionResult ShowCodeEditor()
        {
            //TODO
            return null;
        }

        public ActionResult ShowCommunication()
        {
            return View(OpenProjectModel.Comments);
        }

        public ActionResult ShowGroup()
        {
            return View(OpenProjectModel.Members);
        }

        public ActionResult ShowGoals()
        {
            return View(OpenProjectModel.Goals);
        }

        public ActionResult AddMember(string AspNetUserID)
        {
            projectService.AddUserToProject(AspNetUserID, OpenProjectModel.ID);
            updateUsers();
            return RedirectToAction("ShowGroup", "Project");
        }

        public ActionResult RemoveMember(string AspNetUserID)
        {
            projectService.RemoveUserFromProject(AspNetUserID, OpenProjectModel.ID);
            updateUsers();
            return RedirectToAction("ShowGroup", "Project");
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
            updateGoals();
            return RedirectToAction("ShowGoals", "project");
        }


        public ActionResult RemoveGoal(GoalViewModel goal)
        {
            projectService.RemoveGoal(goal);
            updateGoals();
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
            updateGoals();
            return RedirectToAction("ShowGoals", "Project");
        }

        public ActionResult RemoveObjective(int objectiveID)
        {
            projectService.RemoveObjective(objectiveID);
            updateGoals();
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
        [ValidateAntiForgeryToken]
        public ActionResult CreateFile(FileViewModel fileModel, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    var FileUpload = new File
                    {
                        name = System.IO.Path.GetFileName(upload.FileName),
                        FileType = projectService.GetFileTypeByExtension(System.IO.Path.GetExtension(upload.FileName)),
                        ProjectID = fileModel.ProjectID,
                        HeadFolderID = fileModel.HeadFolderID
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        FileUpload.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    projectService.CreateFile(ref FileUpload);

                    fileModel.ID = FileUpload.ID;

                    TempData["projectModel"] = userHomeService.GetProjectByID(fileModel.ProjectID);
                    return RedirectToAction("Index", "Project");
                } else
                {
                    File newFile = new File()
                    {
                        name = fileModel.name,
                        FileType = projectService.GetFileTypeByID(fileModel.FileTypeID),
                        ProjectID = fileModel.ProjectID,
                        HeadFolderID = fileModel.HeadFolderID
                    };

                    projectService.CreateFile(ref newFile);

                    fileModel.ID = newFile.ID;

                    //   return OpenFile(newFile.ID); eftir að útfæra

                    TempData["projectModel"] = userHomeService.GetProjectByID(fileModel.ProjectID);
                    return RedirectToAction("Index", "Project");
                }
                
            } else
            {
                fileModel = NewFile();
                return View(fileModel);
            }
            
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
            updateFiles();
            return RedirectToAction("Index", "Project", new { id = OpenProjectModel.ID });
        }

        public ActionResult LeaveProject()
        {
            projectService.RemoveUserFromProject(User.Identity.GetUserId(), OpenProjectModel.ID);
            return RedirectToAction("Index", "UserHome");
        }

        public ActionResult ChangeEditorColor()
        {
            //TODO
            return null;
        }
 
        [HttpPost] // can be HttpGet
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
        }

        [HttpPost] // can be HttpGet
        public ActionResult RemoveMemberIfInProject(string email, int projectID)
        {
            bool isValid = projectService.RemoveMemberIfInProject(email, projectID);
            var obj = new
            {
                valid = isValid
            };

            return Json(obj);
        }

    }
}