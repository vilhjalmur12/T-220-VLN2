﻿using CodeEditorApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeEditorApp.Models.ViewModels
{
    public class FileViewModel
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string Content { get; set; }
        public int ProjectID { get; set; }
        public int HeadFolderID { get; set; }
        public int FileTypeID { get; set; }
        public FileType FileType { get; set; }
        public List<SelectListItem> AvailableTypes { get; set; }
    }
}