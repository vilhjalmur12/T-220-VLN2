using System.Collections.Generic;

namespace CodeEditorApp.Models.ViewModels
{
    public class FolderViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProjectID { get; set; }
        public int HeadFolderID { get; set; }
        public bool IsSolutionFolder { get; set; }
        public List<FolderViewModel> SubFolders { get; set; }
        public List<FileViewModel> Files { get; set; }
    }
}