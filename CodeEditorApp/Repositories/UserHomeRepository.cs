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

        public List<ProjectViewModel> GetAllSubProjects(RootFolder folder)
        {

            List<ProjectViewModel> NewModel = new List<ProjectViewModel>();
            _db.Projects.ToList().ForEach((x) =>
            {
                if (x.AspNetUserID == folder.UserID)
                {
                    NewModel.Add(GetProjectByID(x.ID));
                }
            });

            return NewModel;
        }



        /// <summary>
        /// Searches project in database by ID, installs data in a new ViewModel and returns.
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public ProjectViewModel GetProjectByID(int ProjectID)
        {
            Debug.WriteLine("PROJECTID");
            Debug.WriteLine(ProjectID);
            Project project = _db.Projects.Where(x => x.ID == ProjectID).SingleOrDefault();

            ProjectViewModel returnProject = new ProjectViewModel()
            {
                ID = project.ID,
                name = project.name,
                OwnerID = project.AspNetUserID,
                TypeID = project.ProjectTypeID,
                HeadFolderID = project.HeadFolderID,
                SolutionFolderID = project.SolutionFolderID,
                SolutionFolder = GetProjectSolutionFolder(project.SolutionFolderID),
            };

            return returnProject;
        }

        public FolderViewModel GetProjectSolutionFolder(int solutionFolderID)
        {
            Folder solutionFolder = _db.Folders.Where(x => x.ID == solutionFolderID).SingleOrDefault();

            FolderViewModel returnFolder = new FolderViewModel()
            {
                ID = solutionFolder.ID,
                Name = solutionFolder.Name,
                ProjectID = solutionFolder.ProjectID,
                HeadFolderID = solutionFolder.HeadFolderID,
                IsSolutionFolder = solutionFolder.IsSolutionFolder,
                SubFolders = GetAllSubFolders(solutionFolder),
                Files = GetFolderFiles(solutionFolder.ID)
            };
            Debug.WriteLine(solutionFolder.Name);
            Debug.WriteLine("Fjöldi subfolders");
            Debug.WriteLine(returnFolder.SubFolders.Count());

            return returnFolder;
        }




        /// <summary>
        /// Collects all project types for the projects
        /// </summary>
        /// <returns>A list of ProjectType</returns>
        public List<ProjectType> GetAllProjectTypes()
        {
            return _db.ProjectTypes.ToList();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
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
        /// Gets users root folder which holds a project list and a file tree
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public RootFolderViewModel GetUserRootFolder (string UserID)
        {
            RootFolder RootTmp = _db.RootFolders.Where(x => x.UserID == UserID).SingleOrDefault();
            RootFolderViewModel RootFolder = new RootFolderViewModel();

            RootFolder.ID = RootTmp.ID;
            RootFolder.UserID = RootTmp.UserID;

            RootFolder.Projects = GetAllProjects(UserID);
           // RootFolder.Projects = GetAllSubProjects(RootTmp);
            RootFolder.Folders = GetAllSubFolders(RootTmp);

            return RootFolder;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="FolderID"></param>
        /// <returns></returns>
        public FolderViewModel GetFolder (int FolderID) {

            try
            {
                Folder tmp = _db.Folders.Where(x => x.ID == FolderID).SingleOrDefault();
                FolderViewModel folder = new FolderViewModel();

                folder.ID = FolderID;
                folder.Name = tmp.Name;
                folder.ProjectID = tmp.ProjectID;
                folder.HeadFolderID = tmp.HeadFolderID;
                folder.SubFolders = GetAllSubFolders(tmp);
                folder.Files = GetFolderFiles(tmp.ID);
                return folder;
            } catch (EmptyException) { }
            return null;
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
            List<File> fileList = _db.Files.Where(x => x.HeadFolderID == FolderID).ToList();
            List<FileViewModel> returnList = new List<FileViewModel>();

            if (fileList != null)
            {
                foreach (File fileItem in fileList)
                {
                    returnList.Add (new FileViewModel()
                    {
                        ID = fileItem.ID,
                        name = fileItem.name,
                        HeadFolderID = fileItem.HeadFolderID,
                        ProjectID = fileItem.ProjectID
                    });
                }
                return returnList;
            }
            return null;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public List<FolderViewModel> GetAllSubFolders (Folder folder)
        {
            List<Folder> tmpFolders = _db.Folders.Where(x => x.HeadFolderID == folder.ID).ToList();
            List<FolderViewModel> returnList = new List<FolderViewModel>();

            if (tmpFolders != null)
            {
                foreach (Folder folderItem in tmpFolders)
                {
                    returnList.Add(new FolderViewModel
                    {
                        ID = folderItem.ID,
                        Name = folderItem.Name,
                        ProjectID = folderItem.ProjectID,
                        HeadFolderID = folderItem.HeadFolderID,
                        IsSolutionFolder = folderItem.IsSolutionFolder,
                        Files = GetFolderFiles(folderItem.ID),
                        SubFolders = GetAllSubFolders(folderItem)
                    });
                }
                return returnList;
            }

            return null;
        }



        /// <summary>
        /// Overridded function on getting all folders within a folder specially
        /// for the users Root folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public List<FolderViewModel> GetAllSubFolders(RootFolder folder)
        {
            List<Folder> tmpFolders = _db.Folders.Where(x => x.HeadFolderID == 0 && x.AspNetUserID == folder.UserID).ToList();

            List<FolderViewModel> returnList = new List<FolderViewModel>();

            if (tmpFolders != null)
            {
                foreach (Folder folderItem in tmpFolders)
                {
                    returnList.Add(new FolderViewModel()
                    {
                        ID = folderItem.ID,
                        Name = folderItem.Name,
                        ProjectID = folderItem.ProjectID,
                        HeadFolderID = folderItem.HeadFolderID,
                        IsSolutionFolder = folderItem.IsSolutionFolder,
                        Files = GetFolderFiles(folderItem.ID),
                        SubFolders = GetAllSubFolders(folderItem)
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
        public void CreateProject(Project project, Folder HeadFolder)
        {
            project.HeadFolderID = HeadFolder.ID;
            CreateSolutionFolder(ref project);

            _db.Projects.Add(project);
            _db.SaveChanges();

            _db.Folders.Find(project.SolutionFolderID).ProjectID = project.ID;

            Folder TmpFolder = _db.Folders.Where(x => x.Name == project.name + "Solutions" && x.IsSolutionFolder == true).SingleOrDefault();
            Project TmpProject = _db.Projects.Where(x => x.name == project.name + "Solutions" && x.SolutionFolderID == 0).SingleOrDefault();

            TmpFolder.ProjectID = TmpProject.ID;
            TmpProject.SolutionFolderID = TmpFolder.ID;
        }



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
                // CPP.Content = eitthvað

                _db.Files.Add(CPP);
                _db.SaveChanges();
            }
            else if (project.ProjectTypeID == 10) // web
            {
                Folder styles = new Folder()
                {
                    HeadFolderID = project.SolutionFolderID,
                    Name = "styles"
                };

                Folder script = new Folder()
                {
                    HeadFolderID = project.SolutionFolderID,
                    Name = "script"
                };

                File index = new File()
                {
                    HeadFolderID = project.SolutionFolderID,
                    name = "Index",
                    ProjectID = project.ID,
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
                    FileType = _db.FileTypes.Where(x => x.ID == 2).SingleOrDefault()
                };

                File JS = new File()
                {
                    HeadFolderID = script.ID,
                    name = "scripts",
                    ProjectID = project.ID,
                    FileType = _db.FileTypes.Where(x => x.ID == 3).SingleOrDefault()
                };

                
                _db.Files.Add(CSS);
                _db.Files.Add(JS);
                _db.SaveChanges();
            }
        }

        private void CreateCPPFile (Project project, Folder HeadFolder)
        {
            

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public void CreateSolutionFolder(ref Project project)
        {
            //TODO
            Folder NewFolder = new Folder();
            NewFolder.Name = project.name + "Solutions";
            NewFolder.AspNetUserID = project.AspNetUserID;
            NewFolder.ProjectID = 0;
            NewFolder.IsSolutionFolder = true;

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

        public List<SelectListItem> GetFileTypes ()
        {
            List<SelectListItem> ReturnList = new List<SelectListItem>();

            ReturnList.Add(new SelectListItem() { Value = "", Text = "- Select Project Type -" });

            foreach(FileType item in _db.FileTypes.ToList())
            {
                ReturnList.Add(new SelectListItem()
                {
                    Value = item.ID.ToString(),
                    Text = item.Name,
                });
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

                foreach (Comment CommentItem in _db.Comments.Where(x => x.ProjectID == item.ID).ToList())
                {
                    try
                    {
                        _db.Comments.Remove(CommentItem);
                        _db.SaveChanges();
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


        
    }
}