using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class FileViewModel
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int ProjectID { get; set; }
        public int HeadFolderID { get; set; }
    }
}