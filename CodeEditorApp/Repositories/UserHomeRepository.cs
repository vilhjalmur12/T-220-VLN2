using CodeEditorApp.Models;
using CodeEditorApp.Models.Entities;
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

        public List<ProjectViewModel> GetAllProjects(string UserID)
        { 
            
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

            if (NewModel != null)
            {
                foreach (ProjectViewModel project in NewModel)
                {
                    project.Comments = GetProjectComments(project.ID);
                }
            }

            return NewModel;
        }

        public List<Comment> GetProjectComments (int ProjectID)
        {
            List<Comment> NewList = new List<Comment>();
            foreach (Comment comment in _db.Comments.Where(x => x.ProjectID == ProjectID))
            {
                NewList.Add(comment);
            }
            return NewList;
        }

        public List<ProjectType> GetAllProjectTypes ()
        {
            return _db.ProjectTypes.ToList();
        }

        public List<FolderViewModel> GetFileTree(string UserID)
        {
            List<FolderViewModel> UserFolders = new List<FolderViewModel>();
            
            foreach (Folder folder in _db.Folders.Where(x => x.AspNetUserID == UserID))
            {
                UserFolders.Add(new FolderViewModel()
                {
                    ID = folder.ID,
                    Name = folder.Name,

                    //TODO:     project = folder.project
                }); 
            }
            return null;
        }

        public void CreateProject(Project project)
        {
            Folder HeadFolder = CreateSolutionFolder(project);
            Folder tmp = _db.Folders.Where(x => x.ProjectID == project.ID).SingleOrDefault();
            project.HeadFolderID = tmp.ID;

            _db.Projects.Add(project);
            _db.SaveChanges();
        }

        public Folder CreateSolutionFolder(Project project)
        {
            //TODO
            Folder NewFolder = new Folder();
            NewFolder.Name = project.name + "Solutions";
            NewFolder.AspNetUserID = project.AspNetUserID;
            NewFolder.ProjectID = project.ID;

            _db.Folders.Add(NewFolder);

            return NewFolder;
        }

        public void CreateNewSubFolder (string AspNetUserID)
        {

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

        public void CreateRoot(RootFolder Root)
        {
            _db.RootFolders.Add(Root);
            _db.SaveChanges();
        }
    }
}