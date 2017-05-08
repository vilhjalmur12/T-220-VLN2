using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class ProjectTreeViewModel
    {
        public FolderViewModel SolutionFolder { get; set; }
        public List<FolderViewModel> SubFolders { get; set; }
    }
}