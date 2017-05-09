using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class UserViewModel
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public List<ProjectViewModel> Projects { get; set; }
        public RootFolderViewModel RootFolder { get; set; }
        public object Identity { get; internal set; }
    }
}