using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class GoalViewModel
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool finished { get; set; }
        public GoalType goalType { get; set; }
        public string AspNetUserID { get; set; }
        public int ProjectID { get; set; }
        public List<Goal> objectives;
    }
}