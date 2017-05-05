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
                NewList.Add(new CommentViewModel()
                {
                    Content = comment.content,
                    AspNetUserID = comment.AspNetUserID
                });
            }
            return NewList;
        }

        public List<ProjectType> GetAllProjectTypes ()
        {
            return _db.ProjectTypes.ToList();
        }

        public FolderViewModel GetProjectFileTree(Project project)
        {
            Folder tmp = _db.Folders.Where(x => x.ProjectID == project.ID && x.IsSolutionFolder == true).SingleOrDefault();
            FolderViewModel folder = new FolderViewModel();

            folder.ID = tmp.ID;
            folder.Name = tmp.Name;
            folder.ProjectID = tmp.ProjectID;
            folder.HeadFolderID = tmp.HeadFolderID;
            List<FolderViewModel> TmpFolderList = new List<FolderViewModel>();
            GetAllSubFolders(ref TmpFolderList, tmp.ID);
            folder.SubFolders = TmpFolderList;
            
            return null;
        }

        public void GetAllSubFolders (ref List<FolderViewModel> SubFolders, int FolderID)
        {
            List<Folder> TmpFolders = _db.Folders.Where(x => x.HeadFolderID == FolderID).ToList();
            FolderViewModel TmpViewModel = new FolderViewModel();

            if (TmpFolders == null)
            {
                return;
            } else
            {
                foreach (Folder item in TmpFolders)
                {
                    /*
                
                        ID = item.ID,
                        Name = item.Name,
                        ProjectID = item.ProjectID,
                        HeadFolderID = item.HeadFolderID,
                        //TODO GetAllFiles
                     */
                   
                }
            }
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
            List<Folder> SubFolders = _db.Folders.Where(x => x.HeadFolderID == folderID).ToList();

            if (SubFolders == null)
            {
                Folder tmp = _db.Folders.Where(x => x.ID == folderID).SingleOrDefault();
                DeleteFolderFiles(tmp.ID);
                _db.Folders.Remove(tmp);
                _db.SaveChanges();
                return;
            }

            foreach (Folder item in SubFolders)
            {
                DeleteRecursiveFolder(item.ID);
                DeleteFolderFiles(item.ID);
                _db.Folders.Remove(item);
                _db.SaveChanges();
            }   
        }

        public void DeleteProject(int projectID)
        {
            //TODO
            Project RmvProject = _db.Projects.Where(x => x.ID == projectID).SingleOrDefault();
            Folder RmvFolder = _db.Folders.Where(x => x.ID == RmvProject.SolutionFolderID).SingleOrDefault();
            List<Membership> PrjMember = _db.Memberships.Where(x => x.ProjectID == projectID).ToList();

            foreach(Membership item in PrjMember)
            {
                DeleteMembership(item);
            }

            DeleteFolder(RmvFolder.ID);
            DeleteProjectComments(projectID);
            DeleteProjectGoals(projectID);

            _db.Projects.Remove(RmvProject);
            _db.SaveChanges();
        }

        public void DeleteProjectComments (int projectID)
        {
            List<Comment> PrjComments = _db.Comments.Where(x => x.ProjectID == projectID).ToList();

            if (PrjComments != null)
            {
                foreach (Comment item in PrjComments)
                {
                    _db.Comments.Remove(item);
                    _db.SaveChanges();
                }
            }
        }

        public void DeleteProjectGoals (int projectID)
        {
            List<Goal> PrjGoals = _db.Goals.Where(x => x.ProjectID == projectID).ToList();

            if(PrjGoals != null)
            {
                foreach (Goal item in PrjGoals)
                {
                    _db.Goals.Remove(item);
                    _db.SaveChanges();
                }
            }
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

        public void DeleteMembership (Membership membership)
        {
            _db.Memberships.Remove(membership);
            _db.SaveChanges();
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