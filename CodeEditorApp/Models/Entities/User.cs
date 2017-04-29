using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    public class User
    {
        public int userID { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public ICollection<Project> myProjects { get; set; }
        public ICollection<Project> sharedProjects { get; set; }

    }
}