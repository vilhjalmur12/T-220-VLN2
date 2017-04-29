using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    public class File
    {
        public int fileID { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public int projectID { get; set; }
        public virtual Project project { get; set; }
    }
}