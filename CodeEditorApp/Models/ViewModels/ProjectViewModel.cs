using CodeEditorApp.Models;
using CodeEditorApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeEditorApp.Models.ViewModels
{
    public class ProjectViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "A name is required for your project")]
        public string name { get; set; }

        public string OwnerID { get; set; }
        public string Owner { get; set; }

        [Required(ErrorMessage = "A type of project is required")]
        public int TypeID { get; set; }

        public int HeadFolderID { get; set; }
        public int SolutionFolderID { get; set; }
        public FolderViewModel SolutionFolder { get; set; }
        public List<SelectListItem> AvailableProjects { get; set; }
    }
}