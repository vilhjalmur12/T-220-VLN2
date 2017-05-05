using CodeEditorApp.Models;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using CodeEditorApp.Models.Entities;

namespace CodeEditorApp.Repositories
{
    public class ProjectRepository
    {
        private ApplicationDbContext _db;
        public ProjectRepository()
        {
            _db = new ApplicationDbContext();
        }

        public List<GoalViewModel> GetGoalsByProject(int projectID)
        {
            List<GoalViewModel> goalModels = new List<GoalViewModel>();
            _db.Goals.ToList().ForEach(goal =>
            {
                if (goal.ProjectID == projectID)
                {
                    // Get the objectives for the goal
                    List<ObjectiveViewModel> objectiveModels = new List<ObjectiveViewModel>();
                    _db.Objectives.ToList().ForEach(objective =>
                    {
                        if (objective.GoalID == goal.ID)
                        {
                            objectiveModels.Add(new ObjectiveViewModel()
                            {
                                ID = objective.ID,
                                name = objective.name,
                                finished = objective.finished,
                                AspNetUserID = objective.AspNetUserID,
                                GoalID = objective.GoalID
                            });
                        }
                    });

                    goalModels.Add(new GoalViewModel()
                    {
                        ID = goal.ID,
                        name = goal.name,
                        description = goal.description,
                        finished = goal.finished,
                        AspNetUserID = goal.AspNetUserID,
                        ProjectID = goal.ProjectID,
                        objectives = objectiveModels
                    });
                }
            });

            return goalModels;
        }

        public List<CommentViewModel> GetCommentsByProject(int projectID)
        {
            List<CommentViewModel> commentModels = new List<CommentViewModel>();
            _db.Comments.ToList().ForEach(comment =>
            {
                if (comment.ProjectID == projectID)
                {
                    commentModels.Add(new CommentViewModel()
                    {
                        ID = comment.ID,
                        Content = comment.content,
                        AspNetUserID = comment.AspNetUserID,
                        ProjectID = comment.ProjectID,
                    });
                }
            });

            return commentModels;
        }

        public List<UserViewModel> GetUsersByProject(int projectID)
        {
            List<UserViewModel> userModels = new List<UserViewModel>();

            _db.Memberships.ToList().ForEach((x) =>
            {
                if (x.ProjectID == projectID)
                {
                    userModels.Add(GetUser(x.UserID));
                }
            });

            return userModels;
        }


        /// <summary>
        /// Inputs userID and searches for the user linked with the ID. Returns a single user as
        /// ApplicationUser (All information about user).
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>Single ApplicationUser</returns>
        public UserViewModel GetUser(string UserID)
        {
            ApplicationUser TmpUser = _db.Users.Where(x => x.Id == UserID).SingleOrDefault();
            UserViewModel ReturnUser = new UserViewModel();

            ReturnUser.ID = TmpUser.Id;
            ReturnUser.UserName = TmpUser.UserName;

            return ReturnUser;
        }

        public List<FileViewModel> GetFilesByProject(int projectID)
        {
           // TODO
            return null;
        }

        public List<FolderViewModel> GetFoldersByProject (int projectID)
        {
           //TODO
            return null;
        }

        public void AddNewGoal(GoalViewModel goal)
        {
            Goal newGoal = new Goal()
            {
                ID = goal.ID,
                name = goal.name,
                description = goal.description,
                finished = goal.finished,
                AspNetUserID = goal.AspNetUserID,
                ProjectID = goal.ProjectID
            };

            _db.Goals.Add(newGoal);
            _db.SaveChanges();
        }

        public void RemoveGoal(GoalViewModel goal)
        {
            Goal theGoal = _db.Goals.Find(goal.ID);

            if (goal.objectives.Count != 0)
            {
                foreach (ObjectiveViewModel x in goal.objectives)
                {
                    RemoveObjective(x.ID);
                }
            }

            _db.Goals.Remove(theGoal);
            _db.SaveChanges();
        }

        public void AddNewObjective(ObjectiveViewModel objective)
        {
            Objective newObjective = new Objective()
            {
                ID = objective.ID,
                name = objective.name,
                finished = objective.finished,
                AspNetUserID = objective.AspNetUserID,
            };

            _db.Objectives.Add(newObjective);
            _db.SaveChanges();
        }

        public void RemoveObjective(int objectiveID)
        {
            Objective theObjective = _db.Objectives.Find(objectiveID);
            _db.Objectives.Remove(theObjective);
            _db.SaveChanges();
        }

        public void AddUserToProject(string AspNetUserID, int projectID)
        {
            //TODO
        }

        public void RemoveUserFromProject(string AspNetUserID, int projectID)
        {
            //TODO
        }

        public void RemoveFile(int fileID)
        {
            //TODO
        }

        public IEnumerable<FileViewModel> ListFilesByProject(int projectID)
        {
            //TODO
            return null;
        }

        public void AddFileToProject(int projectID)
        {
            //TODO
        }

        public void EditProject(int projectID)
        {
            //TODO
        }

        public void EditFile(int fileID)
        {
            //TODO
        }

        public void EditFolder(int folderID)
        {
            //TODO
        }

        public void AddFileToFolder(int folderID)
        {
            //TODO
        }

        public void ChangeProjectName(int projectID, string newName)
        {
            Project project = _db.Projects.Find(projectID);
        }

        public void ChangeFileName(int fileID, string newName)
        {
            //TODO
        }

        public void ChangeFolderName(int folderID, string newName)
        {
            //TODO
        }
    }
}