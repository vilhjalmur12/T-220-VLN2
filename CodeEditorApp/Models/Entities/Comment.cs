using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string content { get; set; }
        public string userID { get; set; }
        public int projectID { get; set; }
     //   public virtual AspNetUser user { get; set; }
     //   public virtual Project project { get; set; }
    }
}