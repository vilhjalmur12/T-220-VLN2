using System.Collections.Generic;

namespace CodeEditorApp.Models.ViewModels
{
    public class OpenProjectViewModel
    {
        public int ID { get; set; } 
        // ID of the AspNetUser that created the project
        public string OwnerID { get; set; }
        public FolderViewModel SolutionFolder { get; set; } 
        public List<UserViewModel> Members { get; set; } 
        public List<GoalViewModel> Goals { get; set; } 
    }
}