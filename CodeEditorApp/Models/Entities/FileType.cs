using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models.Entities
{
    public class FileType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Details { get; set; }
    }
}