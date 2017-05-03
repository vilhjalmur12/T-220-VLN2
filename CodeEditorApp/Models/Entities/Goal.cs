using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    public enum GoalType
    {
        Goal,
        Objective
    }

    public class Goal
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool finished { get; set; }
        public GoalType goalType { get; set; }
        public int UserID { get; set; }
        public int ProjectID { get; set; }
        public virtual User user { get; set; }
        public virtual Project project { get; set; }
    }
}