using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class MembershipViewModel
    {
        public string AspNetUserID { set; get; }
        public int ProjectID { get; set; }
        public string Email { get; set; }
    }
}