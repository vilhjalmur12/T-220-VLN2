using CodeEditorApp.Models;
using CodeEditorApp.Models.Entities;
using CodeEditorApp.Models.ViewModels;
using CodeEditorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Repositories
{
    
    public class UserHomeRepository
    {
        public ApplicationDbContext _db = new ApplicationDbContext();


        public List<Project> GetAllProjects(string UserID)
        {

            /*
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
            */
            /*
            List<Project> tmp = _db.Projects.Where(x => x.AspNetUserID == UserID).ToList();
            ProjectViewModel tmpProject = new ProjectViewModel();

            foreach (Project project in tmp)
            {
                
                tmpProject.ID = project.ID;
                tmpProject.name = project.name;
                foreach (Folder folder in _db.Folders.Where(x => x.ProjectID == project.ID))
                {
                    tmpProject.Folders.Add(folder);
                }
                foreach (Comment comment in _db.Comments.Where(x => x.projectID == project.ID))
                {
                    tmpProject.Comments.Add(comment);
                }
                NewModel.Add(tmpProject);
            }
            */
            return _db.Projects.ToList();
        }

        
    }
}