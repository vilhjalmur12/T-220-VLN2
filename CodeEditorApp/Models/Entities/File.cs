using CodeEditorApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [StringLength(255)]
        public string name { get; set; }
        
        /// <summary>
        /// The ID of the Project that the File belongs to. 
        /// </summary>
        public int ProjectID { get; set; }

        public string Content { get; set; }

        public int HeadFolderID { get; set; }

        public virtual FileType FileType { get; set; }
     //   public virtual Project project { get; set; }
     //   public virtual Folder folder { get; set; }
    }
}