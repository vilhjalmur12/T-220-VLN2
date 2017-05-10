using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class OpenProjectViewModel
    {
        public int ID { get; set; } 
        public string name { get; set; } 
        public FolderViewModel SolutionFolder { get; set; } 
        public List<CommentViewModel> Comments { get; set; } 
        public List<UserViewModel> Members { get; set; } 
        public List<GoalViewModel> Goals { get; set; } 
    }
}