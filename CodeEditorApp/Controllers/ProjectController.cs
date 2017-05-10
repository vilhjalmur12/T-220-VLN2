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
        private UserHomeRepository userHomeService = new UserHomeRepository();

        private OpenProjectViewModel OpenProjectModel;

        [HttpGet]
        public ActionResult Index(int? projectID)
        {
            //projectModel = (ProjectViewModel)TempData["projectModel"];
            // projectModel = projectService.GetOpenProjectViewModel(projectID.Value);
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
                AvailableTypes = userHomeService.GetFileTypes(),
                HeadFolderID = OpenProjectModel.SolutionFolderID
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
        public ActionResult CreateFile(FileViewModel fileModel)
        {
            Debug.WriteLine(fileModel.ProjectID);
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
            return RedirectToAction("Index", "Project", new { model = OpenProjectModel });
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
            bool isValid = projectService.AddMemberIfExists(email, projectID); //.. check
            var obj = new
            {
                valid = isValid
            };
            return Json(obj);
        }

    }
}