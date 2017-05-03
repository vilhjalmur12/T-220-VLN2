using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    public class File
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public int ProjectID { get; set; }
     //   public virtual Project project { get; set; }
    }
}