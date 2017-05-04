using CodeEditorApp.Models;
using CodeEditorApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class ProjectViewModel
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public int TypeID { get; set; }
        public Folder HeadFolder { get; set; }
        public List<Folder> Folders { get; set; }
        public List<Comment> Comments { get; set; }
    }
}