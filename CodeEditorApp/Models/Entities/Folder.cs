using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.Entities
{
    public class Folder
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int ProjectID { get; set; }
    }
}