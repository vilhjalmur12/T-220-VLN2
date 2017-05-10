using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.Entities
{
    public class Membership
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string AspNetUserID { get; set; }
    }
}