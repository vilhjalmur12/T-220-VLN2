﻿using CodeEditorApp.Models;
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

        public void CreateFile(ref File file)
        {
            _db.Files.Add(file);
            _db.SaveChanges();
        }

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


        //Hér sækjum við lista af notendum eftir projectID
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

        public List<FileViewModel> GetFilesByProject(int projectID)
        {
            // TODO
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

        public List<FolderViewModel> GetFoldersByProject (int projectID)
        {
            //TODO
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

        public void RemoveGoal(GoalViewModel goalModel)
        {
            Goal theGoal = _db.Goals.Find(goalModel.ID);

            if (theGoal != null)
            {
                _db.Goals.Remove(theGoal);
                _db.SaveChanges();
            }
        }

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

        //Removes user from project
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

        public IEnumerable<FileViewModel> ListFilesByProject(int projectID)
        {
            //TODO
            return null;
        }

        public void AddFileToProject(int projectID)
        {
            //TODO
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


        // Adds user to project if user exists. Returns true if user was added to project, else returns false.
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

        private Project FindProjectByID(int projectID)
        {
            return _db.Projects.Find(projectID);
        }

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

        private ApplicationUser FindUserByEmail(string email)
        {
            ApplicationUser myUser = _db.Users.SingleOrDefault(user => user.Email == email);
            return myUser;
        }

        /*public bool RemoveMemberIfInProject(string email, int projectID)
        {
            List<UserViewModel> userList = GetUsersByProject(projectID); //sæki lista af notendum sem eru í þessu projectID-i

            foreach (UserViewModel user in userList)
            {
                if (user.UserName == email)
                {
                    RemoveUserFromProject(user.ID, projectID); //eyðum notanda úr verkefni - veit ekki hvort að ég megi nota þetta userid
                }
                return true;
            }
            return false;
        }*/

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