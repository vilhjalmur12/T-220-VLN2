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

namespace CodeEditorApp.Controllers
{
    public class ProjectController: Controller
    {
        private ProjectRepository project = new ProjectRepository();

        private ProjectViewModel projectModel;

        public ActionResult Index(ProjectViewModel model)
        {
            updateComments();
            updateGoals();
            updateUsers();
            return View();
        }

        private void updateGoals()
        {
            projectModel.Goals = project.GetGoalsByProject(projectModel.ID);
        }

        private void updateComments()
        {
            projectModel.Comments = project.GetCommentsByProject(projectModel.ID);
        }

        private void updateUsers()
        {
            projectModel.Members = project.GetUsersByProject(projectModel.ID);
        }

        public ActionResult ShowCodeEditor()
        {
            //TODO
            return null;
        }

        public ActionResult ShowCommunication()
        {
            //TODO
            return null;
        }

        public ActionResult ShowGroup()
        {
            //TODO
            return null;
        }

        public ActionResult ShowGoals()
        {
            //TODO
            return null;
        }

        public ActionResult AddMember(string AspNetUserID)
        {
            project.AddUserToProject(AspNetUserID, projectModel.ID);
            updateUsers();
            return RedirectToAction("ShowGroup", "Project");
        }

        public ActionResult RemoveMember(string AspNetUserID)
        {
            project.RemoveUserFromProject(AspNetUserID, projectModel.ID);
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

            GoalViewModel thisGoal = new GoalViewModel() { name = goalName, description = goalDescription, ProjectID = projectModel.ID, AspNetUserID = User.Identity.GetUserName(), finished = false, goalType = GoalType.Goal };
            project.AddNewGoal(thisGoal);
            updateGoals();
            return RedirectToAction("ShowGoals", "project");
        }

        public ActionResult RemoveGoal(int goalID)
        {
            project.RemoveGoal(goalID);
            updateGoals();
            return RedirectToAction("ShowGoals", "project");
        }

        public ActionResult AddObjective(int goalID, string userID, FormCollection collection)
        {
            string objectiveName = collection["objectiveName"];
            string objectiveDescription = collection["objectiveDescription"];

            if (String.IsNullOrEmpty(objectiveName))
            {
                return View("Error");
            }
            if (String.IsNullOrEmpty(objectiveDescription))
            {
                return RedirectToAction("Index", "Project", new { id = projectModel.ID });
            }

            GoalViewModel thisObjective = new GoalViewModel() { name = objectiveName, description = objectiveDescription, ProjectID = projectModel.ID, AspNetUserID = userID, finished = false, goalType = GoalType.Objective };
            project.AddNewObjective(thisObjective);
            updateGoals();
            return RedirectToAction("ShowGoals", "project");
        }

        public ActionResult RemoveObjective(int objectiveID)
        {
            project.RemoveObjective(objectiveID);
            updateGoals();
            return RedirectToAction("ShowGoals", "project");
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
            //TODo
            return null;
        }

        public ActionResult LeaveProject()
        {
            //TODO
            return null;
        }

        public ActionResult ChangeEditorColor()
        {
            //TODO
            return null;
        }
    }
}