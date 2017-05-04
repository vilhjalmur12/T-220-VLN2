using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.Entities
{
    public class Folder
    {
        /// <summary>
        /// ID of the folder
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name of the folder
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ID of the user OWNER
        /// </summary>
        public string AspNetUserID { get; set; }

        public int ProjectID { get; set; }
        public int HeadFolderID { get; set; }

        public bool IsSolutionFolder { get; set; }
    //    public virtual AspNetUser user { get; set; }
    //    public virtual Project project { get; set; }
    }
}