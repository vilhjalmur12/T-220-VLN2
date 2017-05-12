using CodeEditorApp.Models;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using CodeEditorApp.Repositories;
using Microsoft.AspNet.Identity;
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
            // If tabMake is null: display default
            if (tabMake == null)
            {
                tabMake = "";
            }

            OpenProjectModel = projectService.GetOpenProjectViewModel(projectID.Value);
            ViewBag.newFile = CreateNewFileModel();
            ViewBag.newMembership = CreateNewMembershipModel();
            ViewBag.newGoal = CreateNewGoalModel();
            //For the Editor
            List<FileViewModel> AllSolutionFiles = projectService.GetFilesByProject(projectID.Value);
            ViewBag.AllSolutionFiles = AllSolutionFiles;
            ViewBag.Code = "alert('Hello World!');";
            ViewBag.DocumentID = 0;
            //ViewBag.ProjectID = projectID;
            ViewBag.UserName = User.Identity.GetUserName();
            ViewBag.UseID = User.Identity.GetUserId();
            //For the editor
            ViewBag.tabMake = tabMake;

            return View(OpenProjectModel);
        }

        /// <summary>
        /// Creates a new FileViewModel with available fileTypes,
        /// the ID and the headFolder ID of the open project,
        /// </summary>
        /// <returns> FileViewModel </returns>
        private FileViewModel CreateNewFileModel()
        {
            FileViewModel newFile = new FileViewModel()
            {
                ProjectID = OpenProjectModel.ID,
                AvailableTypes = projectService.GetFileTypes(),
                HeadFolderID = OpenProjectModel.SolutionFolder.ID
            };

            return newFile;
        }

        /// <summary>
        /// Creates a new MembershipViewModel with te current Project ID
        /// </summary>
        /// <returns> MembershipViewModel </returns>
        private MembershipViewModel CreateNewMembershipModel()
        {
            MembershipViewModel newMembershipModel = new MembershipViewModel()
            {
                ProjectID = OpenProjectModel.ID
            };

            return newMembershipModel;
        }

        /// <summary>
        /// Creates a new GoalViewModel with the current projectID
        /// and the logged in userID
        /// </summary>
        /// <returns></returns>
        private GoalViewModel CreateNewGoalModel()
        {
            GoalViewModel newGoalModel = new GoalViewModel()
            {
                ProjectID = OpenProjectModel.ID,
                AspNetUserID = User.Identity.GetUserId()
            };

            return newGoalModel;
        }

        /// <summary>
        /// Removes membership from database
        /// </summary>
        /// <param name="membership"></param>
        /// <returns> ActionResult </returns>
        public ActionResult RemoveMember(MembershipViewModel membershipModel)
        {
            projectService.RemoveMemberFromProject(membershipModel);
            return RedirectToAction("Index", "Project", new { projectID = membershipModel.ProjectID, tabMake = "project-members"});
        }

        /// <summary>
        /// Creates a new Goal in database based on the GoalViewModel goal
        /// </summary>
        /// <param name="goal"></param>
        /// <returns> Action Result </returns>
        [HttpPost]
        public ActionResult NewGoal(GoalViewModel goalModel)
        {
            // Check if goal is empty
            if ((goalModel.name != null) && (goalModel.name.Length > 0) )
            {
                GoalViewModel newGoalModel = new GoalViewModel()
                {
                    AspNetUserID = User.Identity.GetUserId(),
                    name = goalModel.name,
                    description = goalModel.description,
                    ProjectID = goalModel.ProjectID,
                    finished = false
                };

                projectService.AddGoal(newGoalModel);
            }
            
            return RedirectToAction("Index", "Project", new { projectID = goalModel.ProjectID, tabMake = "project-goals" });
        }

        /// <summary>
        /// Removes the goal represented by the GoalViewmodel goal
        /// </summary>
        /// <param name="goal"></param>
        /// <returns> Action Result </returns>
        public ActionResult RemoveGoal(GoalViewModel goalModel)
        {
            projectService.RemoveGoal(goalModel);
            return RedirectToAction("Index", "Project", new { projectID = goalModel.ProjectID, tabMake = "project-goals"});
        }

        //public ActionResult NewObjective(ObjectiveViewModel objectiveModel)
        //{
        //    ObjectiveViewModel newObjective = new ObjectiveViewModel()
        //    {
        //        name = name,
        //        GoalID = goalID,
        //        AspNetUserID = User.Identity.GetUserId(),
        //        finished = false
        //    };

        //    projectService.AddNewObjective(newObjective);
        //    return RedirectToAction("Index", "Project", new { projectID = projectID, tabMake = "project-goals" });
        //}

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
            FileViewModel newFile = CreateNewFileModel();
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
                fileModel = CreateNewFileModel();
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
            projectService.RemoveMemberFromProject(membership);

            return RedirectToAction("Index", "UserHome");
        }


        public ActionResult ChangeEditorColor()
        {
            //TODO
            return null;
        }
 
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

        [HttpPost]
        public void SaveFile (string documentID, string fileContent)
        {
            int intDocumentID = Convert.ToInt32(documentID);
            projectService.SaveFileContent(intDocumentID, fileContent);
        }
    }
}