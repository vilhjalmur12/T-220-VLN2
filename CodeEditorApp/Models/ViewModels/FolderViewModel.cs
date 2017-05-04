using CodeEditorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeEditorApp.Models.Entities;

namespace CodeEditorApp.Models.ViewModels
{
    public class FolderViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Project project { get; set; }
        public List<FolderViewModel> SubFolders { get; set; }
        public List<FileViewModel> Files { get; set; }
    }
}