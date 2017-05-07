using CodeEditorApp.Models;
using CodeEditorApp.Models.Entities;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace CodeEditorApp.Repositories
{
    
    public class UserHomeRepository
    {
        private ApplicationDbContext _db;



        /// <summary>
        /// The class initializer will create a NEW instance of ApplicationDbContext to 
        /// connect to our database.
        /// </summary>
        public UserHomeRepository()
        {
            _db = new ApplicationDbContext();
        }


        /// <summary>
        /// A method to call all projects owned by a specific user with specific UserID, takes all projects
        /// and transforms the Projects to a ViewModel class ("ProjectViewModel") and returns that list.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>A list of ProjectViewModels owned or linked with user</returns>
        public List<ProjectViewModel> GetAllProjects(string UserID)
        { 
            
            List<ProjectViewModel> NewModel = new List<ProjectViewModel>();
            _db.Projects.ToList().ForEach((x) =>
            {
                if (x.AspNetUserID == UserID)
                {
                    NewModel.Add(GetProjectByID(x.ID));
                }
            });

            return NewModel;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public ProjectViewModel GetProjectByID (int ProjectID)
        {
            ProjectViewModel ReturnProject = new ProjectViewModel();
            Project tmp = _db.Projects.Where(x => x.ID == ProjectID).SingleOrDefault();

            ReturnProject.ID = tmp.ID;
            ReturnProject.name = tmp.name;
          //  ReturnProject.OwnerID = tmp.AspNetUserID;
           // ReturnProject.TypeID = tmp.ProjectTypeID;
           // ReturnProject.HeadFolderID = tmp.HeadFolderID;
           // ReturnProject.SolutionFolderID = tmp.SolutionFolderID;
            //ReturnProject.SolutionFolder = GetFolder(ReturnProject.SolutionFolderID);
            //ReturnProject.Comments = GetProjectComments(ReturnProject.ID);
            //ReturnProject.Members = GetProjectMembers(ReturnProject.ID);
           // ReturnProject.Goals = GetProjectGoals(ReturnProject.ID);

            return ReturnProject;
        }



        /// <summary>
        /// Collects all project types for the projects
        /// </summary>
        /// <returns>A list of ProjectType</returns>
        public List<ProjectType> GetAllProjectTypes()
        {
            return _db.ProjectTypes.ToList();
        }

        public List<UserViewModel> GetProjectMembers (int ProjectID)
        {
            List<UserViewModel> ReturnList = new List<UserViewModel>();
            UserViewModel TmpUser = new UserViewModel();

            foreach (Membership tableitem in _db.Memberships.Where(x => x.ProjectID == ProjectID))
            {
                TmpUser = GetUser(tableitem.UserID);
                ReturnList.Add(TmpUser);
            }

            return ReturnList;
        }

        public List<GoalViewModel> GetProjectGoals (int ProjectID)
        {
            List<GoalViewModel> ReturnList = new List<GoalViewModel>();
            GoalViewModel TmpGoal = new GoalViewModel();

            foreach(Goal goalItem in _db.Goals.Where(x => x.ProjectID == ProjectID))
            {
                TmpGoal.ID = goalItem.ID;
                TmpGoal.name = goalItem.name;
                TmpGoal.description = goalItem.description;
                TmpGoal.ProjectID = goalItem.ProjectID;
                TmpGoal.AspNetUserID = goalItem.AspNetUserID;
                TmpGoal.finished = goalItem.finished;
                ReturnList.Add(TmpGoal);
            }

            return ReturnList;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
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
        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public FolderViewModel GetProjectFileTree(Project project)
        {
            Folder tmp = _db.Folders.Where(x => x.ProjectID == project.ID && x.IsSolutionFolder == true).SingleOrDefault();
            FolderViewModel folder = new FolderViewModel();

            folder.ID = tmp.ID;
            folder.Name = tmp.Name;
            folder.ProjectID = tmp.ProjectID;
            folder.HeadFolderID = tmp.HeadFolderID;
            folder.SubFolders =  GetAllSubFolders(tmp);
            folder.Files = GetFolderFiles(tmp.ID);

            return null;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="FolderID"></param>
        /// <returns></returns>
        public FolderViewModel GetFolder (int FolderID) {
            Folder tmp = _db.Folders.Where(x => x.ID == FolderID).SingleOrDefault();
            FolderViewModel folder = new FolderViewModel();

            folder.ID = tmp.ID;
            folder.Name = tmp.Name;
            folder.ProjectID = tmp.ProjectID;
            folder.HeadFolderID = tmp.HeadFolderID;
            folder.SubFolders = GetAllSubFolders(tmp);
            folder.Files = GetFolderFiles(tmp.ID);
            return folder;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileID"></param>
        /// <returns></returns>
        public FileViewModel GetFile (int FileID)
        {
            File tmp = _db.Files.Where(x => x.ID == FileID).SingleOrDefault();
            FileViewModel NewFile = new FileViewModel();

            NewFile.ID = tmp.ID;
            NewFile.name = tmp.name;
            NewFile.HeadFolderID = tmp.HeadFolderID;
            NewFile.ProjectID = tmp.ProjectID;
            return NewFile;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="FolderID"></param>
        /// <returns></returns>
        public List<FileViewModel> GetFolderFiles (int FolderID)
        {
            Folder tmp = _db.Folders.Where(x => x.ID == FolderID).SingleOrDefault();
            List<File> DbFileList = _db.Files.Where(x => x.HeadFolderID == FolderID).ToList();
            List<FileViewModel> NewList = new List<FileViewModel>();

            if (DbFileList != null)
            {
                foreach (File item in DbFileList)
                {
                    NewList.Add(new FileViewModel()
                    {
                        ID = item.ID,
                        name = item.name,
                        HeadFolderID = item.HeadFolderID,
                        ProjectID = item.ProjectID
                    });
                }
                return NewList;
            }
            else
            {
                return null;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public List<FolderViewModel> GetAllSubFolders (Folder folder)
        {
            List<Folder> TmpFolders = _db.Folders.Where(x => x.HeadFolderID == folder.ID).ToList();
            List<FolderViewModel> NewList = new List<FolderViewModel>();
            FolderViewModel TmpViewModel = new FolderViewModel();

            if (TmpFolders == null)
            {
                return null;
            } else
            {
                foreach (Folder item in TmpFolders)
                {
                    TmpViewModel.ID = item.ID;
                    TmpViewModel.Name = item.Name;
                    TmpViewModel.ProjectID = item.ProjectID;
                    TmpViewModel.HeadFolderID = item.HeadFolderID;
                    TmpViewModel.Files = GetFolderFiles(item.ID);
                    TmpViewModel.SubFolders = GetAllSubFolders(item);
                    NewList.Add(TmpViewModel);
                }
                return NewList;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        public void CreateProject(Project project)
        {
            Folder HeadFolder = CreateSolutionFolder(project);
            Folder tmp = _db.Folders.Where(x => x.ProjectID == project.ID).SingleOrDefault();
            project.HeadFolderID = tmp.ID;

            _db.Projects.Add(project);
            _db.SaveChanges();

            
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="AspNetUserID"></param>
        /// <param name="HeadFolderID"></param>
        /// <param name="folder"></param>
        public void CreateFolder (string AspNetUserID, int HeadFolderID, Folder folder)
        {
            folder.AspNetUserID = AspNetUserID;
            folder.HeadFolderID = HeadFolderID;

            _db.Folders.Add(folder);
            _db.SaveChanges();

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public void CreateFile (FileViewModel file)
        {
            File NewFile = new Models.File();

            NewFile.name = file.name;
            NewFile.HeadFolderID = file.HeadFolderID;
            NewFile.ProjectID = file.ProjectID;

            _db.Files.Add(NewFile);
            _db.SaveChanges();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderID"></param>
        public void DeleteFolder(int folderID)
        {
            DeleteRecursiveFolder(folderID);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderID"></param>
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileID"></param>
        public void DeleteFile(int fileID)
        {
            //TODO
            File RemoveFile = _db.Files.Where(x => x.ID == fileID).SingleOrDefault();

            _db.Files.Remove(RemoveFile);
            _db.SaveChanges();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderID"></param>
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="membership"></param>
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Root"></param>
        public void CreateRoot(RootFolder Root)
        {
            _db.RootFolders.Add(Root);
            _db.SaveChanges();
        }



        /// <summary>
        /// Inputs userID and searches for the user linked with the ID. Returns a single user as
        /// ApplicationUser (All information about user).
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>Single ApplicationUser</returns>
        public UserViewModel GetUser (string UserID)
        {
            ApplicationUser TmpUser = _db.Users.Where(x => x.Id == UserID).SingleOrDefault();
            UserViewModel ReturnUser = new UserViewModel();

            ReturnUser.ID = TmpUser.Id;
            ReturnUser.UserName = TmpUser.UserName;

            return ReturnUser;
        }
    }
}