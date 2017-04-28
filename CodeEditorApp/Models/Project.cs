using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public int ownerID { get; set; }
        public int projectTypeID { get; set; }
        public virtual User owner { get; set; }
        public virtual ProjectType projectType { get; set; }
        public virtual ICollection<User> users { get; set; }
        public virtual ICollection<File> files { get; set; }
        public virtual ICollection<Goal> ownerGoals { get; set; }
        public virtual ICollection<Goal> userGoals { get; set; }
        public virtual ICollection<Comment> comments { get; set; }
    }
}