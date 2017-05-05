using CodeEditorApp.Models;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

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
            List<GoalViewModel> NewModel = new List<GoalViewModel>();
            _db.Goals.ToList().ForEach((x) =>
            {
                if (x.ProjectID == projectID)
                {
                    List<ObjectiveViewModel> theObjectives = new List<ObjectiveViewModel>();
                    _db.Objectives.ToList().ForEach((y) =>
                    {
                        if (y.GoalID == x.ID)
                        {
                            theObjectives.Add(new ObjectiveViewModel()
                            {
                                ID = y.ID,
                                name = y.name,
                                finished = y.finished,
                                AspNetUserID = y.AspNetUserID,
                                GoalID = y.GoalID
                            });
                        }
                    });
                    NewModel.Add(new GoalViewModel()
                    {
                        ID = x.ID,
                        name = x.name,
                        description = x.description,
                        finished = x.finished,
                        AspNetUserID = x.AspNetUserID,
                        ProjectID = x.ProjectID,
                        objectives = theObjectives
                    });
                }
            });

            return NewModel;
        }

        public List<CommentViewModel> GetCommentsByProject(int projectID)
        {
            List<CommentViewModel> NewModel = new List<CommentViewModel>();
            _db.Comments.ToList().ForEach((x) =>
            {
                if (x.ProjectID == projectID)
                {
                    NewModel.Add(new CommentViewModel()
                    {
                        ID = x.ID,
                        Content = x.content,
                        AspNetUserID = x.AspNetUserID,
                        ProjectID = x.ProjectID,
                    });
                }
            });

            return NewModel;
        }

        public List<UserViewModel> GetUsersByProject(int projectID)
        {
            List<UserViewModel> NewModel = new List<UserViewModel>();
            _db.Memberships.ToList().ForEach((x) =>
            {
                if (x.ProjectID == projectID)
                {
                    NewModel.Add(new UserViewModel()
                    {                  
                       
                    });
                }
            });

            return NewModel;
        }

        public List<FileViewModel> GetFilesByProject(int ProjectID)
        {
            
            return null;
        }

        public List<FolderViewModel> GetFoldersByProject (int ProjectID)
        {
            //TODO
            return null;
        }

        public void AddNewGoal(GoalViewModel goal)
        {
            //TODO
        }

        public void RemoveGoal(int goalID)
        {
            //TODO
        }

        public void AddNewObjective(ObjectiveViewModel objective)
        {
            //TODO
        }

        public void RemoveObjective(int objectiveID)
        {
            //TODO
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

        public IEnumerable<CommentViewModel> ListCommentsByProject(int projectID)
        {
            //TODO
            return null;
        }


        public IEnumerable<GoalViewModel> ListGoalsByProject(int projectID)
        {
            //TODO
            return null;
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
            //TODO
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