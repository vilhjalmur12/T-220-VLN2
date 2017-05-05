﻿using CodeEditorApp.Models;
using CodeEditorApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class ProjectViewModel
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string OwnerID { get; set; }
        public int TypeID { get; set; }
        public int HeadFolderID { get; set; }
        public int SolutionFolderID { get; set; }
        public FolderViewModel SolutionFolder { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public List<UserViewModel> Members { get; set; }
        public List<GoalViewModel> Goals { get; set; }  
    }
}