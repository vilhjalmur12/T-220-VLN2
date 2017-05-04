using CodeEditorApp.Models;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeEditorApp.Repositories;
using Microsoft.AspNet.Identity;

namespace CodeEditorApp.Controllers
{
    public class ProjectController: Controller
    {
        private ProjectRepository project = new ProjectRepository();

        private int projectID;
        private List<GoalViewModel> projectGoals = new List<GoalViewModel>();
        private List<CommentViewModel> projectComments = new List<CommentViewModel>();
        private List<AspNetUser> projectUsers = new List<AspNetUser>();

        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                projectID = id.Value;
                updateComments();
                updateGoals();
                updateUsers();
                return View();
            }
            else
            {
                return View("Error");
            }
        }

        private void updateGoals()
        {
            project.GetGoalsByProject(projectID);
        }

        private void updateComments()
        {
            project.GetCommentsByProject(projectID);
        }

        private void updateUsers()
        {
            project.GetUsersByProject(projectID);
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
            project.AddUserToProject(AspNetUserID);
            updateUsers();
            return null;
        }

        public ActionResult RemoveMember(string AspNetUserID)
        {
            project.RemoveUserFromProject(AspNetUserID);
            return null;
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
                return RedirectToAction("Index", "Project", new { id = projectID });
            }

            string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            GoalViewModel thisGoal = new GoalViewModel() { name = goalName, description = goalDescription, ProjectID = projectID, AspNetUserID = User.Identity.GetUserName(), finished = false, goalType = 0 };
            project.AddNewGoal(thisGoal);
            updateGoals();
            return RedirectToAction("ShowGoals", "project");
        }

        public ActionResult RemoveGoal(int goalID)
        {
            project.RemoveGoal(goalID);
            return RedirectToAction("ShowGoals", "project");
        }

        public ActionResult AddObjective(int goalID)
        {
            //TODO
            return null;
        }

        public ActionResult RemoveObjective(int objectiveID)
        {
            //TODO
            return null;
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