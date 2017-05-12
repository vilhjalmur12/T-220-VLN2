using System.Collections.Generic;

namespace CodeEditorApp.Models.ViewModels
{
    public class RootFolderViewModel
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public List<FolderViewModel> Folders { get; set; }
    }
}