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

        public void CreateFile(ref File file)
        {
            _db.Files.Add(file);
            _db.SaveChanges();
        }

        public void ChangeGoal(int goalID)
        {
            if (_db.Goals.Find(goalID).finished)
            {
                _db.Goals.Find(goalID).finished = false;
            }
            else
            {
                _db.Goals.Find(goalID).finished = true;
            }
            _db.SaveChanges();
        }

        public OpenProjectViewModel GetOpenProjectViewModel(int projectID)
        {
            Project project = _db.Projects.Find(projectID);
            OpenProjectViewModel newOpenProjectModel = new OpenProjectViewModel()
            {
                ID = projectID,
                name = project.name,
                SolutionFolder = GetSolutionFolder(project.SolutionFolderID),
                Comments = GetCommentsByProject(projectID),
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
                    // Get the objectives for the goal
                    List<ObjectiveViewModel> objectiveModels = new List<ObjectiveViewModel>();
                    _db.Objectives.ToList().ForEach(objective =>
                    {
                        if (objective.GoalID == goal.ID)
                        {
                            objectiveModels.Add(new ObjectiveViewModel()
                            {
                                ID = objective.ID,
                                name = objective.name,
                                finished = objective.finished,
                                AspNetUserID = objective.AspNetUserID,
                                GoalID = objective.GoalID
                            });
                        }
                    });

                    goalModels.Add(new GoalViewModel()
                    {
                        ID = goal.ID,
                        name = goal.name,
                        description = goal.description,
                        finished = goal.finished,
                        AspNetUserID = goal.AspNetUserID,
                        ProjectID = goal.ProjectID,
                        objectives = objectiveModels
                    });
                }
            });

            return goalModels;
        }


        public List<CommentViewModel> GetCommentsByProject(int projectID)
        {
            List<CommentViewModel> commentModels = new List<CommentViewModel>();
            _db.Comments.ToList().ForEach(comment =>
            {
                if (comment.ProjectID == projectID)
                {
                    commentModels.Add(new CommentViewModel()
                    {
                        ID = comment.ID,
                        Content = comment.content,
                        AspNetUserID = comment.AspNetUserID,
                        ProjectID = comment.ProjectID,
                    });
                }
            });

            return commentModels;
        }

        //Hér sækjum við lista af notendum eftir projectID
        public List<UserViewModel> GetUsersByProject(int projectID)
        {
            List<UserViewModel> userModels = new List<UserViewModel>();
            _db.Memberships.ToList().ForEach(membership =>
            {
                if (membership.ProjectID == projectID)
                {
                    userModels.Add(GetUser(membership.AspNetUserID));
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
        public UserViewModel GetUser(string UserID)
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
                        name = file.name,
                        ProjectID = file.ProjectID,
                        HeadFolderID = file.HeadFolderID,
                    });
                }
            });
            return fileModels;
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

        public void AddNewGoal(GoalViewModel goal)
        {
            Goal newGoal = new Goal()
            {
                name = goal.name,
                description = goal.description,
                finished = goal.finished,
                AspNetUserID = goal.AspNetUserID,
                ProjectID = goal.ProjectID
            };

            _db.Goals.Add(newGoal);
            _db.SaveChanges();
        }

        public void RemoveGoal(GoalViewModel goal)
        {
            Goal theGoal = _db.Goals.Find(goal.ID);

            if (goal.objectives.Count != 0)
            {
                foreach (ObjectiveViewModel x in goal.objectives)
                {
                    RemoveObjective(x.ID);
                }
            }

            _db.Goals.Remove(theGoal);
            _db.SaveChanges();
        }

        public void AddNewObjective(ObjectiveViewModel objective)
        {
            Objective newObjective = new Objective()
            {
                name = objective.name,
                finished = objective.finished,
                AspNetUserID = objective.AspNetUserID,
            };

            _db.Objectives.Add(newObjective);
            _db.SaveChanges();
        }

        public void RemoveObjective(int objectiveID)
        {
            Objective theObjective = _db.Objectives.Find(objectiveID);
            _db.Objectives.Remove(theObjective);
            _db.SaveChanges();
        }

        public void AddNewComment(CommentViewModel comment)
        {
            Comment newComment = new Comment()
            {
                AspNetUserID = comment.AspNetUserID,
                content = comment.Content,
                ProjectID = comment.ProjectID
            };

            _db.Comments.Add(newComment);
            _db.SaveChanges();
        }


        public void AddUserToProject(string AspNetUserID, int projectID)
        {
            Membership newMembership = new Membership()
            {
                ProjectID = projectID,
                AspNetUserID = AspNetUserID,
            };
            _db.Memberships.Add(newMembership);
            _db.SaveChanges();
        }

        public void RemoveUserFromProject (string AspNetUserID, int projectID)
        {
            _db.Memberships.ToList().ForEach(membership =>
            {
                if ((membership.ProjectID == projectID) && (membership.AspNetUserID == AspNetUserID))
                {
                    Membership deleteMembership = membership;
                    _db.Memberships.Remove(deleteMembership);
                    _db.SaveChanges();
                }
            });
        }

        public void RemoveFile(int fileID)
        {
            //TODO
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

        public void ChangeProjectName(int projectID, string newName)
        {
            Project project = _db.Projects.Find(projectID);
        }

        public void ChangeFileName(int fileID, string newName)
        {
            //TODO
        }

        public void ChangeFolderName(int folderID, string newName)
        {
            //TODO
        }

        /*public UserViewModel GetUserByEmail(string email)
        {
            ApplicationUser TmpUser = _db.Users.Where(x => x.UserName == email).SingleOrDefault();
            UserViewModel ReturnUser = new UserViewModel();

            ReturnUser.ID = TmpUser.Id;
            ReturnUser.UserName = TmpUser.UserName;

            return ReturnUser;
        }*/

        // Adds user to project if user exists. Returns true if user was added to project, else returns false.
        public bool AddMemberIfExists(string email, int projectID)
        {
            List<ApplicationUser> users = _db.Users.ToList();
            foreach (ApplicationUser user in users)
            {
                if (user.Email == email)
                {
                    AddUserToProject(user.Id, projectID);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveMemberIfInProject(string email, int projectID)
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
        }
        
    }
}