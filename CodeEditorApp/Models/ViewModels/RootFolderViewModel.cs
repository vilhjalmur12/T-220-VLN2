using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class RootFolderViewModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public List<ProjectViewModel> Projects { get; set; }
        public List<FolderViewModel> Folders { get; set; }
    }
}