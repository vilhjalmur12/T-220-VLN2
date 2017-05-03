using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    public class AspNetUser
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public virtual ICollection<Project> myProjects { get; set; }
    }
}