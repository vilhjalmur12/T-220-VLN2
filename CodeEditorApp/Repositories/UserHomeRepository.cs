using CodeEditorApp.Models;
using CodeEditorApp.Models.Entities;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Data.Entity;
using System.Diagnostics;

namespace CodeEditorApp.Repositories
{
    // Execptions used
    public class EmptyException : Exception { }


    
    public class UserHomeRepository: TreeRepository
    {
        private ApplicationDbContext _db;

        public UserHomeRepository()
        {
            _db = new ApplicationDbContext();
        }
        
        /// <summary>
        /// Returns a list of ProjectViewModel
        /// Represent all projects the user has access to
        /// Uses GetProjectModelByProject() and GetProjectByID()
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<ProjectViewModel> GetAllProjects(string userID)
        { 
            List<ProjectViewModel> projectModelList = new List<ProjectViewModel>();

            // Get users projects
            _db.Projects.ToList().ForEach(project =>
            {
                if (project.AspNetUserID == userID)
                {
                    projectModelList.Add(GetProjectModelByProject(project));
                }
            });

            // Get projects the user has been added to
            _db.Memberships.ToList().ForEach(membership =>
            {
                if (membership.AspNetUserID == userID)
                {
                    projectModelList.Add(GetProjectByID(membership.ProjectID));
                }
            });

            return projectModelList;
        }

        /// <summary>
        /// Returns a ProjectViewModel that represents the Project project
        /// Uses GetFolderByID()
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        private ProjectViewModel GetProjectModelByProject(Project project)
        {
            ProjectViewModel returnProjectModel = new ProjectViewModel()
            {
                ID = project.ID,
                Name = project.name,
                Owner = _db.Users.Find(project.AspNetUserID).UserName,
                TypeID = project.ProjectTypeID,
                HeadFolderID = project.HeadFolderID,
                SolutionFolderID = project.SolutionFolderID,
                SolutionFolder = GetFolderByID(project.SolutionFolderID)
            };

            return returnProjectModel;
        }

        /// <summary>
        /// Returns a FolderViewModel that represents the Folder folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private FolderViewModel GetFolderViewModelByFolder(Folder folder)
        {
            FolderViewModel returnFolder = new FolderViewModel()
            {
                ID = folder.ID,
                Name = folder.Name,
                ProjectID = folder.ProjectID,
                HeadFolderID = folder.HeadFolderID,
                IsSolutionFolder = folder.IsSolutionFolder,
                SubFolders = GetAllSubFolders(folder.ID),
                Files = GetAllSubFiles(folder.ID)
            };

            return returnFolder;
        }

        /// <summary>
        /// Searches project in database by ID, installs data in a new ViewModel and returns.
        /// Uses GetProjectModelByProject()
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        private ProjectViewModel GetProjectByID(int ProjectID)
        {
            Project project = _db.Projects.Where(p => p.ID == ProjectID).SingleOrDefault();
            return GetProjectModelByProject(project);
        }

        /// <summary>
        /// Searches folder in database by ID, installs the data in a new ViewModel and return.
        /// Uses GetFolderViewModelByFolder
        /// </summary>
        /// <param name="solutionFolderID"></param>
        /// <returns></returns>
        private FolderViewModel GetFolderByID(int folderID)
        {
            Folder folder = _db.Folders.Where(f => f.ID == folderID).SingleOrDefault();
            return GetFolderViewModelByFolder(folder);
        }


        /// <summary>
        /// Collects all project types for the projects
        /// </summary>
        /// <returns>A list of ProjectType</returns>
        private List<ProjectType> GetAllProjectTypes()
        {
            return _db.ProjectTypes.ToList();
        }

        /// <summary>
        /// Gets users root folder which holds a project list and a file tree
        /// Uses GetSubFoldersForRoot
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public RootFolderViewModel GetUserRootFolder (string UserID)
        {
            RootFolder rootFolder = _db.RootFolders.Where(x => x.UserID == UserID).SingleOrDefault();
            RootFolderViewModel returnRootFolder = new RootFolderViewModel()
            {
                ID = rootFolder.ID,
                UserID = rootFolder.UserID,
                Folders = GetSubFoldersForRoot(rootFolder)
            };

            return returnRootFolder;
        }


        /// <summary>
        /// Gets all subFolders for RootFolder folder
        /// Uses GetAllSubFiles() and GetAllSubFolders()
        /// for the users Root folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private List<FolderViewModel> GetSubFoldersForRoot(RootFolder rootFolder)
        {
            List<Folder> folders = _db.Folders.Where(f => f.HeadFolderID == 0 && f.AspNetUserID == rootFolder.UserID).ToList();

            List<FolderViewModel> returnList = new List<FolderViewModel>();

            if (folders != null)
            {
                foreach (Folder folder in folders)
                {
                    returnList.Add(new FolderViewModel()
                    {
                        ID = folder.ID,
                        Name = folder.Name,
                        ProjectID = folder.ProjectID,
                        HeadFolderID = folder.HeadFolderID,
                        IsSolutionFolder = folder.IsSolutionFolder,
                        Files = GetAllSubFiles(folder.ID),
                        SubFolders = GetAllSubFolders(folder.ID)
                    });
                }

                return returnList;
            }

            return null;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
       /* public void CreateProject(Project project, Folder headFolder)
        {
            project.HeadFolderID = headFolder.ID;
            CreateSolutionFolder(ref project);
            _db.Projects.Add(project);
            _db.SaveChanges();
            _db.Folders.Find(project.SolutionFolderID).ProjectID = project.ID;

            Folder tmpSolutionFolder = _db.Folders.Where(x => x.Name == project.name + "Solutions" && x.IsSolutionFolder == true).SingleOrDefault();
            Project tmpProject = _db.Projects.Where(x => x.name == project.name + "Solutions" && x.SolutionFolderID == 0).SingleOrDefault();

          //  tmpSolutionFolder.ProjectID = tmpProject.ID;
            tmpProject.SolutionFolderID = tmpSolutionFolder.ID;
        }*/



        /// <summary>
        /// Overridded for input of only project, in case of creating a project
        /// under root folder.
        /// </summary>
        /// <param name="project"></param>
        public void CreateProject(ref Project project)
        {
            CreateSolutionFolder(ref project);
            project.HeadFolderID = 0;

            _db.Projects.Add(project);
            _db.SaveChanges();

            _db.Folders.Find(project.SolutionFolderID).ProjectID = project.ID;
            GenerateProjectFiles(project);
        }

        private void GenerateProjectFiles (Project project)
        {
            if (project.ProjectTypeID == 4) //console
            {
                File CPP = new File();
                CPP.name = "main";
                CPP.ProjectID = project.ID;
                CPP.HeadFolderID = project.SolutionFolderID;
                CPP.FileType = _db.FileTypes.Where(x => x.ID == 4).SingleOrDefault();
                CPP.Content = "// CPP File";

                _db.Files.Add(CPP);
                _db.SaveChanges();
            }
            else if (project.ProjectTypeID == 10) // web
            {
                Folder styles = new Folder()
                {
                    HeadFolderID = project.SolutionFolderID,
                    Name = "styles",
                    AspNetUserID = project.AspNetUserID,
                    ProjectID = project.ID
                };

                Folder script = new Folder()
                {
                    HeadFolderID = project.SolutionFolderID,
                    Name = "script",
                    AspNetUserID = project.AspNetUserID,
                    ProjectID = project.ID
                };

                File index = new File()
                {
                    HeadFolderID = project.SolutionFolderID,
                    name = "Index",
                    ProjectID = project.ID,
                    Content = "<!-- This is Html -->",
                    FileType = _db.FileTypes.Where(x => x.ID == 1).SingleOrDefault()
                };

                _db.Folders.Add(styles);
                _db.Folders.Add(script);
                _db.Files.Add(index);
                _db.SaveChanges();

                File CSS = new File()
                {
                    HeadFolderID = styles.ID,
                    name = "styles",
                    ProjectID = project.ID,
                    Content = "/* This is Css */",
                    FileType = _db.FileTypes.Where(x => x.ID == 2).SingleOrDefault()
                };

                File JS = new File()
                {
                    HeadFolderID = script.ID,
                    name = "scripts",
                    Content = "// This is Javascript",
                    ProjectID = project.ID,
                    FileType = _db.FileTypes.Where(x => x.ID == 3).SingleOrDefault()
                };

                
                _db.Files.Add(CSS);
                _db.Files.Add(JS);
                _db.SaveChanges();
            } else
            {
                File index = new Models.File
                {
                    HeadFolderID = project.HeadFolderID,
                    name = "index",
                    ProjectID = project.ID,
                    FileType = _db.FileTypes.Where(x => x.ID == 6).SingleOrDefault()
                };

                _db.Files.Add(index);
                _db.SaveChanges();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public void CreateSolutionFolder(ref Project project)
        {
            Folder NewFolder = new Folder()
            {
                Name = project.name + "Solutions",
                AspNetUserID = project.AspNetUserID,
                ProjectID = project.ID,
                IsSolutionFolder = true
            };

            _db.Folders.Add(NewFolder);
            _db.SaveChanges();

            project.SolutionFolderID = NewFolder.ID;
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
            DeleteProjectGoals(projectID);

            _db.Projects.Remove(RmvProject);
            _db.SaveChanges();
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

        
        public List<SelectListItem> GetProjectTypes ()
        {
            List<SelectListItem> ReturnList = new List<SelectListItem>();

            ReturnList.Add(new SelectListItem() { Value="", Text="- Select Project Type -" });
            
            foreach(ProjectType item in _db.ProjectTypes.ToList())
            {
                ReturnList.Add(new SelectListItem() { Value = item.ID.ToString(), Text = item.name });
            }

            return ReturnList;
        }

        public void ClearUserData(string UserID)
        {
            foreach (Project item in _db.Projects.Where(x => x.AspNetUserID == UserID).ToList())
            {
                foreach (File FileItem in _db.Files.Where(x => x.ProjectID == item.ID).ToList())
                {
                    try
                    {
                        _db.Files.Remove(FileItem);
                        _db.SaveChanges();
                    }
                    catch (EmptyException) { } 
                }

                foreach (Folder FolderItem in _db.Folders.Where(x => x.ProjectID == item.ID && x.IsSolutionFolder == true).ToList())
                {
                    try
                    {
                        DeleteRecursiveFolder(FolderItem.ID);
                    } catch (EmptyException) { } 
                }

                foreach (Goal GoalItem in _db.Goals.Where(x => x.ProjectID == item.ID).ToList())
                {
                    try
                    {
                        _db.Goals.Remove(GoalItem);
                        _db.SaveChanges();
                    } catch (EmptyException) { }
                }
            }

            foreach (Folder item in _db.Folders.Where(x => x.AspNetUserID == UserID).ToList())
            {
                try
                {
                    _db.Folders.Remove(item);
                    _db.SaveChanges();
                } catch (EmptyException) { }
            }
        }

        public void RemoveFile(int fileID)
        {
            //TODO
            File RFile = _db.Files.Where(x => x.ID == fileID).SingleOrDefault();
            if (RFile != null)
            {
                _db.Files.Remove(RFile);
                _db.SaveChanges();
            }
        }



    }
}