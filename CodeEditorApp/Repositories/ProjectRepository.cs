using CodeEditorApp.Models;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Repositories
{
    public class ProjectRepository
    {
        public ProjectRepository()
        {
        }

        public List<GoalViewModel> GetGoalsByProject(int ProjectID)
        {
            //Todo
            return null;
        }

        public List<CommentViewModel> GetCommentsByProject(int ProjectID)
        {
            //Todo
            return null;
        }

        public List<UserViewModel> GetUsersByProject(int ProjectID)
        {
            //Todo
            return null;
        }

        public void AddUserToProject(string AspNetUserID)
        {
            //TODO
        }

        public void RemoveUserFromProject(string AspNetUserID)
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

        public IEnumerable<ObjectiveViewModel> ListObjectivesByProject(int projectID)
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