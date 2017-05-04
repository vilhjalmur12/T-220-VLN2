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

        public List<ProjectViewModel> GetAllProjects(string AspNetUserID)
        { 
            /*
            List<ProjectViewModel> NewModel = new List<ProjectViewModel>();
            _db.Projects.ToList().ForEach((x) =>
            {
                if (x.AspNetUserID == UserID)
                {
                    NewModel.Add(new ProjectViewModel()
                    {
                        ID = x.ID,
                        name = x.name,
                    });
                }
            });
            */
            /*
            List<Project> tmp = _db.Projects.Where(x => x.AspNetUserID == UserID).ToList();
            ProjectViewModel tmpProject = new ProjectViewModel();

            foreach (Project project in tmp)
            {
                
                tmpProject.ID = project.ID;
                tmpProject.name = project.name;
                foreach (Folder folder in _db.Folders.Where(x => x.ProjectID == project.ID))
                {
                    tmpProject.Folders.Add(folder);
                }
                foreach (Comment comment in _db.Comments.Where(x => x.projectID == project.ID))
                {
                    tmpProject.Comments.Add(comment);
                }
                NewModel.Add(tmpProject);
            }
            */
            return _db.Projects.ToList();
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