using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.Entities
{
    public class Objective
    {  
        public int ID { get; set; }
        public string name { get; set; }
        public bool finished { get; set; }
        public string AspNetUserID { get; set; }
        public int GoalID { get; set; }
    }
}