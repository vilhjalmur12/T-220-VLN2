using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    /// <summary>
    /// A Project represents the project that a user creates.
    /// A Project may contain multiple files.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// The database generates unique ID of the project.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The name of the Project, Example: "Project1".
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The ID of the user that created the project.
        /// </summary>
        public string AspNetUserID { get; set; }
        /// <summary>
        /// The ID of the type of the project.
        /// </summary>
        public int ProjectTypeID { get; set; }
        /// <summary>
        /// The ID of the HEAD folder
        /// </summary>
        public int FolderID { get; set; }

     //   public virtual AspNetUser user { get; set; }
     //   public virtual ProjectType projectType { get; set; }
    }
}