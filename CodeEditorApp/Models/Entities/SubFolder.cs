using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.Entities
{
    public class SubFolder
    {
        /// <summary>
        /// ID of the subfolder
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Name of the subfolder
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The ID of the user OWNER
        /// </summary>
        public string AspNetUserID { get; set; }
        /// <summary>
        /// The ID of its HEAD folder
        /// </summary>
        public int FolderID { get; set; }
    }
}