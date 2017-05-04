using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    /// <summary>
    /// A comment represents a comment from a user.
    /// The comment is shown in the chat window for the Project.
    /// The comment is shown with the user name of the user.
    /// Each project may contain multiple comments.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// The database generates unique ID of the comment
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The content is the comment it self.
        /// This content apears in the chat.
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// The ID of the user that made the comment.
        /// </summary>
        public string AspNetUserID { get; set; }
        /// <summary>
        /// The ID of the project that has the comment
        /// </summary>
        public int ProjectID { get; set; }
     //   public virtual AspNetUser user { get; set; }
     //   public virtual Project project { get; set; }
    //    public virtual AspNetUser user { get; set; }
    //    public virtual Project project { get; set; }

    }
}