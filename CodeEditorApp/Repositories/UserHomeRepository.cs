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

        public List<CommentViewModel> GetProjectComments (int ProjectID)
        {
            List<CommentViewModel> NewList = new List<CommentViewModel>();
            foreach (Comment comment in _db.Comments.Where(x => x.ProjectID == ProjectID))
            {
              //  NewList.Add(comment);
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
            NewFolder.IsSolutionFolder = true;

            _db.Folders.Add(NewFolder);
            _db.SaveChanges();

            return NewFolder;
        }

        public void CreateFolder (string AspNetUserID, int HeadFolderID, Folder folder)
        {
            folder.AspNetUserID = AspNetUserID;
            folder.HeadFolderID = HeadFolderID;

            _db.Folders.Add(folder);
            _db.SaveChanges();

        }
        public void DeleteFolder(int folderID)
        {
            DeleteRecursiveFolder(folderID);
        }

        public void DeleteRecursiveFolder (int folderID)
        {
            List<Folder> SubFolders = _db.Folders.Where(x => x.ID == folderID).ToList();

            if (SubFolders == null)
            {
                return;
            }

            foreach (Folder item in SubFolders)
            {
                DeleteRecursiveFolder(item.ID);
                Folder RemoveFolder = _db.Folders.Where(x => x.ID == folderID).SingleOrDefault();
                _db.Folders.Remove(RemoveFolder);
                _db.SaveChanges();
            }   
        }

        public void DeleteProject(int projectID)
        {
            //TODO

            
        }

        public void DeleteFile(int fileID)
        {
            //TODO
            File RemoveFile = _db.Files.Where(x => x.ID == fileID).SingleOrDefault();

            _db.Files.Remove(RemoveFile);
            _db.SaveChanges();
        }

        public void DeleteFolderFiles (int folderID)
        {
            List<File> AllFiles = new List<File>();
            AllFiles = _db.Files.Where(x => x.HeadFolderID == folderID).ToList();

            if (AllFiles != null)
            {
                foreach (File item in AllFiles)
                {
                    _db.Files.Remove(item);
                    _db.SaveChanges();
                }
            }
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