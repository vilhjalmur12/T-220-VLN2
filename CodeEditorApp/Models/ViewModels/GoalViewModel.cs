using System.Collections.Generic;

namespace CodeEditorApp.Models.ViewModels
{
    public class GoalViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Finished { get; set; }
        public string AspNetUserID { get; set; }
        public int ProjectID { get; set; }
    }
}