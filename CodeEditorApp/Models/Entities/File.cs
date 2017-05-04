using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    /// <summary>
    /// A File represents a file that is in a Project.
    /// Project may contain multiple files.
    /// Files can, for example, contained html code or c++ code.
    /// </summary>
    public class File
    {
        /// <summary>
        /// The database generates unique ID of the File.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The name of the file, Example:CommentViewModel.cs
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The location of the file.
        /// Example: "username/folder1/project1"
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// The ID of the Project that the File belongs to. 
        /// </summary>
        public int ProjectID { get; set; }
     //   public virtual Project project { get; set; }
    }
}