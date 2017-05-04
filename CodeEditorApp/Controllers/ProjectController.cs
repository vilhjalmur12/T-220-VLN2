using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeEditorApp.Controllers
{
    public class ProjectController: Controller
    {
        public ActionResult Index()
        {
            return View();
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
            //TODO
            return null;
        }

        public ActionResult RemoveMember(string AspNetUserID)
        {
            //TODO
            return null;
        }

        public ActionResult AddGoal(string AspNetUserID)
        {
            //TODO
            return null;
        }

        public ActionResult RemoveGoal(int goalID)
        {
            //TODO
            return null;
        }

        public ActionResult AddObjective(int goalID, string AspNetUserID)
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