using CodeEditorApp.Models;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using CodeEditorApp.Models.Entities;
using System.Diagnostics;

namespace CodeEditorApp.Repositories
{
    public class ProjectRepository: TreeRepository
    {
        private ApplicationDbContext _db;
        public ProjectRepository()
        {
            _db = new ApplicationDbContext();
        }

        public FileType GetFileTypeByID(int fileTypeID)
        {
            return _db.FileTypes.Find(fileTypeID);
        }

        public FileType GetFileTypeByExtension(string Ext)
        {
            return _db.FileTypes.Where(x => x.Extension == Ext).SingleOrDefault();
        }
        /// <summary>
        ///Creates a file 
        /// </summary>
        /// <param name="file"></param>
        public void CreateFile(ref File file)
        {
            _db.Files.Add(file);
            _db.SaveChanges();
        }
        /// <summary>
        /// Get the view model from the project open
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>returns the project open</returns>
        public OpenProjectViewModel GetOpenProjectViewModel(int projectID)
        {
            Project project = _db.Projects.Find(projectID);
            OpenProjectViewModel newOpenProjectModel = new OpenProjectViewModel()
            {
                ID = projectID,
                OwnerID = project.AspNetUserID,
                SolutionFolder = GetSolutionFolder(project.SolutionFolderID),
                Members = GetUsersByProject(projectID),
                Goals = GetGoalsByProject(projectID)
            };

            return newOpenProjectModel;
        }

        /// <summary>
        /// Gets the solution folder desired by ID
        /// </summary>
        /// <param name="solutionFolderID"></param>
        /// <returns></returns>
        public FolderViewModel GetSolutionFolder(int solutionFolderID)
        {
            Folder solutionFolder = _db.Folders.Find(solutionFolderID);

            FolderViewModel newSolutionFolderViewModel = new FolderViewModel()
            {
                ID = solutionFolderID,
                Name = solutionFolder.Name,
                HeadFolderID = solutionFolder.HeadFolderID,
                SubFolders = GetAllSubFolders(solutionFolderID),
                Files = GetAllSubFiles(solutionFolderID)
            };

            return newSolutionFolderViewModel;
        }

        /// <summary>
        /// Gets the goals listed in the project by project id
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>List of the goals in the project</returns>
        public List<GoalViewModel> GetGoalsByProject(int projectID)
        {
            List<GoalViewModel> goalModels = new List<GoalViewModel>();
            _db.Goals.ToList().ForEach(goal =>
            {
                if (goal.ProjectID == projectID)
                {
                    goalModels.Add(new GoalViewModel()
                    {
                        ID = goal.ID,
                        Name = goal.name,
                        Description = goal.description,
                        Finished = goal.finished,
                        AspNetUserID = goal.AspNetUserID,
                        ProjectID = goal.ProjectID
                    });
                }
            });

            return goalModels;
        }

        /// <summary>
        /// Here we get a list of members in a project 
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>The list of the members in wanted project ID</returns>
        public List<UserViewModel> GetUsersByProject(int projectID)
        {
            List<UserViewModel> userModels = new List<UserViewModel>();
            _db.Memberships.ToList().ForEach(membership =>
            {
                if (membership.ProjectID == projectID)
                {
                    userModels.Add(GetUserByID(membership.AspNetUserID));
                }
            });

            return userModels;
        }


        /// <summary>
        /// Inputs userID and searches for the user linked with the ID. Returns a single user as
        /// ApplicationUser (All information about user).
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns>Single ApplicationUser</returns>
        public UserViewModel GetUserByID(string UserID)
        {
            ApplicationUser TmpUser = _db.Users.Where(x => x.Id == UserID).SingleOrDefault();
            UserViewModel ReturnUser = new UserViewModel();

            ReturnUser.ID = TmpUser.Id;
            ReturnUser.UserName = TmpUser.UserName;

            return ReturnUser;
        }

        /// <summary>
        /// Get files from a desired projectID 
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>Returns a list of files in the projectID desired</returns>
        public List<FileViewModel> GetFilesByProject(int projectID)
        {
            List<FileViewModel> fileModels = new List<FileViewModel>();
            _db.Files.ToList().ForEach(file =>
            {
                if (file.ProjectID == projectID)
                {
                    fileModels.Add(new FileViewModel()
                    {
                        ID = file.ID,
                        Name = file.name,
                        ProjectID = file.ProjectID,
                        HeadFolderID = file.HeadFolderID,
                    });
                }
            });
            return fileModels;
        }

        /// <summary>
        /// Get file with desired ID
        /// </summary>
        /// <param name="FileID"></param>
        /// <returns>Returns the file wanted</returns>
        public FileViewModel GetFileByID (int FileID)
        {
            File file = _db.Files.Where(x => x.ID == FileID).SingleOrDefault();

            FileViewModel ReturnFile = new FileViewModel
            {
                ID = file.ID,
                Name = file.name,
                ProjectID = file.ProjectID,
                HeadFolderID = file.HeadFolderID,
                FileType = file.FileType,
                Content = file.Content
            };

            return ReturnFile;
        }

        /// <summary>
        /// Get folders in the project you are in
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>Returns a list of a folders in project</returns>
        public List<FolderViewModel> GetFoldersByProject (int projectID)
        {
            List<FolderViewModel> folderModels = new List<FolderViewModel>();
            _db.Folders.ToList().ForEach(folder =>
            {
                if (folder.ProjectID == projectID)
                {
                    folderModels.Add(new FolderViewModel()
                    {
                        ID = folder.ID,
                        Name = folder.Name,
                        ProjectID = folder.ProjectID,
                        HeadFolderID = folder.HeadFolderID,
                    });
                }


            });
            return folderModels;
        }

        /// <summary>
        /// Adds a new goal to project
        /// </summary>
        /// <param name="goalModel"></param>
        public void AddGoal(GoalViewModel goalModel)
        {
            Goal newGoal = new Goal()
            {
                name = goalModel.Name,
                description = goalModel.Description,
                finished = goalModel.Finished,
                AspNetUserID = goalModel.AspNetUserID,
                ProjectID = goalModel.ProjectID
            };

            _db.Goals.Add(newGoal);
            _db.SaveChanges();
        }

        /// <summary>
        /// Removes a goal from a project
        /// </summary>
        /// <param name="goalModel"></param>
        public void RemoveGoal(GoalViewModel goalModel)
        {
            Goal theGoal = _db.Goals.Find(goalModel.ID);

            if (theGoal != null)
            {
                _db.Goals.Remove(theGoal);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// This function adds user to a project
        /// </summary>
        /// <param name="membershipModel"></param>
        public void AddUserToProject(MembershipViewModel membershipModel)
        {
            Membership newMembership = new Membership()
            {
                ProjectID = membershipModel.ProjectID,
                AspNetUserID = membershipModel.AspNetUserID,
            };
            _db.Memberships.Add(newMembership);
            _db.SaveChanges();
        }

        /// <summary>
        /// This function removes user from a project if
        /// </summary>
        /// <param name="membership"></param>
        public void RemoveMemberFromProject (MembershipViewModel membership)
        {

            List<Membership> membershipList = _db.Memberships.ToList();
            foreach (Membership member in membershipList)
            {
                if (member.AspNetUserID == membership.AspNetUserID && member.ProjectID == membership.ProjectID)
                {
                    _db.Memberships.Remove(member);
                    _db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Removes a file
        /// </summary>
        /// <param name="fileID"></param>
        public void RemoveFile(int fileID)
        {
            File RFile = _db.Files.Where(x => x.ID == fileID).SingleOrDefault();
            if (RFile != null)
            {
                _db.Files.Remove(RFile);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// Adds member to a project if user is a registered user and is not a member already
        /// </summary>
        /// <param name="membershipModel"></param>
        public void AddMemberIfExists(MembershipViewModel membershipModel)
        {
            ApplicationUser user = FindUserByEmail(membershipModel.Email);
            Project project = FindProjectByID(membershipModel.ProjectID);

            if ((user != null) && (project != null))
            {
                membershipModel.AspNetUserID = user.Id;
                if ((!MembershipExists(membershipModel)) && (project.AspNetUserID != membershipModel.AspNetUserID))
                {
                    AddUserToProject(membershipModel);
                }
            }
        }
        /// <summary>
        /// Finds project in the database by the ID of the project
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns>Returns the </returns>
        private Project FindProjectByID(int projectID)
        {
            return _db.Projects.Find(projectID);
        }

        /// <summary>
        /// Finds if user is already a member in a project
        /// </summary>
        /// <param name="membershipModel"></param>
        /// <returns>returns true if user is a member and false if not</returns>
        private bool MembershipExists(MembershipViewModel membershipModel)
        {
            foreach (Membership membership in _db.Memberships.ToList())
            {
                if (membership.AspNetUserID == membershipModel.AspNetUserID && membership.ProjectID == membershipModel.ProjectID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds user by email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns>returns the user with desired email</returns>
        private ApplicationUser FindUserByEmail(string email)
        {
            ApplicationUser myUser = _db.Users.SingleOrDefault(user => user.Email == email);
            return myUser;
        }


        /// <summary>
        /// Saves the content in a file
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="content"></param>
        public void SaveFileContent (int fileID, string content)
        {
            if (content == null)
            {
                content = "";
            }
            if (fileID != 0)
            {
                
                File Tmp = _db.Files.Where(x => x.ID == fileID).SingleOrDefault();
                if (Tmp == null)
                {
                    return;
                }
                Debug.WriteLine("File Content Before: " + Tmp.Content);
                Tmp.Content = content;
                _db.SaveChanges();
                Debug.WriteLine("File Content After: " + Tmp.Content);
            } 
        }
        
    }
}