using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class CommentViewModel
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public string AspNetUserID { get; set; }
        public int ProjectID { get; set; }
    }
}