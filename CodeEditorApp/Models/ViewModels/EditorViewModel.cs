using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.ViewModels
{
    public class EditorViewModel
    {
        public string Content { get; set; }

        public string ID { get; set; }

        public int OwnerId { get; set; }
    }
}