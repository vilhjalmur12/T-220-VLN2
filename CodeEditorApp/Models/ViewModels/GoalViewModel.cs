using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class GoalViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public bool Checked { get; set; }
        public GoalType Type { get; set; }
        public AspNetUser User { get; set; }
        public int ProjectID { get; set; }
    }
}