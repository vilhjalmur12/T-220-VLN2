using CodeEditorApp.Models;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using CodeEditorApp.Models.Entities;

namespace CodeEditorApp.Repositories
{
    public class ProjectRepository
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
                    userModels.Add(GetUser(membership.UserID));
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
                UserID = AspNetUserID,
            };
            _db.Memberships.Add(newMembership);
            _db.SaveChanges();
        }

       /* public void RemoveUserFromProject(ref ProjectViewModel project, string AspNetUserID) //Project View Model, þar er ég með projID og lista af öllum memberum
        {
            //TODO
            //UserViewModel theUser = project.Members.Find(AspNetUserID);

            List<UserViewModel> theUsers = GetUsersByProject(project.ID); //GetUsersByProject skilar lista af user-um sem eru í þessu projectID.
            //ÓKLÁRAÐ
            Membership theMembership = _db.Memberships.Find(AspNetUserID);
            _db.Memberships.Remove(theMembership);
            _db.SaveChanges();

        }*/

        public void RemoveUserFromProject (string AspNetUserID, int projectID)
        {
            _db.Memberships.ToList().ForEach(membership =>
            {
                if ((membership.ProjectID == projectID) && (membership.UserID == AspNetUserID))
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

        public bool AddMemberIfExists(string email, int projectID) {
            //Veit ekki hvort að ég má gera þetta?!
            foreach (ApplicationUser user in _db.Users) {
                if (user.Email == email) {
                    AddUserToProject(user.Id, projectID);
                    return true;
                }
            }
            return false;
        }
    }
}