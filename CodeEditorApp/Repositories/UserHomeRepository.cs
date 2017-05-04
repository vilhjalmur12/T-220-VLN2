using CodeEditorApp.Models;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Repositories
{
    
    public class UserHomeRepository
    {
        private ApplicationDbContext _db;

        public UserHomeRepository()
        {
            _db = new ApplicationDbContext();
        }

        public IEnumerator<ProjectViewModel> GetAllProjects(string AspNetUserID)
        {
            //TODO
            return null;
        }

        public IEnumerator<FolderViewModel> GetFileTree(string AspNetUserID)
        {
            //TODO
            return null;
        }

        public void CreateProject(string AspNetUserID)
        {
            //TODO
        }

        public void CreateNewFolder(string AspNetUserID)
        {
            //TODO
        }

        public void DeleteProject(int projectID)
        {
            //TODO
        }

        public void DeleteFile(int fileID)
        {
            //TODO
        }

        public void MoveProjectPath(int projectID, string newPath)
        {
            //TODO
        }

        public void MoveFolderPath(int folderID, string newPath)
        {
            //TODO
        }

        public void MoveFilePath(int fileID, string newPath)
        {
            //TODO
        }

        public void SendConfirmEmail(string AspNetUserID)
        {
            //TODO
        }

        public bool EmailConfirmed(string AspNetUserID)
        {
            //TODO
            return false;
        }
    }
}