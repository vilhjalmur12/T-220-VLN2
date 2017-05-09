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

        private ProjectViewModel projectModel;

        [HttpGet]
        public ActionResult Index()
        {
            projectModel = (ProjectViewModel)TempData["projectModel"];
            updateComments();
            updateGoals();
            updateUsers();
            //For the Editor
            ViewBag.Code = "alert('Hello World!');";
            ViewBag.DocumentID = 17;
            ViewBag.UserName = User.Identity.GetUserName();
            ViewBag.UseID = User.Identity.GetUserId();
            //For the editor

            return View(projectModel);
        }

        [HttpPost]
        public ActionResult ChangeGoal (int? goalID)
        {
            projectService.ChangeGoal(goalID.Value);
            updateGoals();
            return RedirectToAction("Index", "Project", projectModel);
        }

        private void updateGoals()
        {
            projectModel.Goals = projectService.GetGoalsByProject(projectModel.ID);
        }

        private void updateComments()
        {
            projectModel.Comments = projectService.GetCommentsByProject(projectModel.ID);
        }

        private void updateUsers()
        {
            projectModel.Members = projectService.GetUsersByProject(projectModel.ID);
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
            return View(projectModel.Comments);
        }

        public ActionResult ShowGroup()
        {
            return View(projectModel.Members);
        }

        public ActionResult ShowGoals()
        {
            return View(projectModel.Goals);
        }

        public ActionResult AddMember(string AspNetUserID)
        {
            projectService.AddUserToProject(AspNetUserID, projectModel.ID);
            updateUsers();
            return RedirectToAction("ShowGroup", "Project");
        }

        public ActionResult RemoveMember(string AspNetUserID)
        {
            projectService.RemoveUserFromProject(AspNetUserID, projectModel.ID);
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
                return RedirectToAction("Index", "Project", new { id = projectModel.ID });
            }

            GoalViewModel newGoal = new GoalViewModel()
            {
                name = goalName,
                description = goalDescription,
                ProjectID = projectModel.ID,
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
                    ProjectID = projectModel.ID
                };

                projectService.AddNewComment(commentModel);
            }
        }

        public ActionResult CreateFile()
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
            return RedirectToAction("Index", "Project", new { model = projectModel });
        }

        public ActionResult LeaveProject()
        {
            projectService.RemoveUserFromProject(User.Identity.GetUserId(), projectModel.ID);
            return RedirectToAction("Index", "UserHome");
        }

        public ActionResult ChangeEditorColor()
        {
            //TODO
            return null;
        }
        
    }
}