using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    /// <summary>
    /// ProjectType represents the type of a project.
    /// </summary>
    public class ProjectType
    {
        /// <summary>
        /// The database generates unique ID of the projectType.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The title of the projectType, Example: "Console".
        /// </summary>
        public string name { get; set; }
    }
}