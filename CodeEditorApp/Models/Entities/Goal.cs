using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Models
{
    /// <summary>
    /// enum for the goalType
    /// 0: Goal
    /// 1: Objective
    /// Each Goal may contain multiple Objectives
    /// </summary>
    public enum GoalType
    {
        Goal,
        Objective
    }

    /// <summary>
    /// A Goal represents a goal or an objective for a Project.
    /// Each Project can contain multiple Goals.
    /// </summary>
    public class Goal
    {
        /// <summary>
        /// The database generates unique ID of the Goal
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// The name of the Goal.
        /// Example: "Design system"
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The description of the Goal.
        /// Example: "We need to work hard to finish this goal"
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Finished represents if the Goal has been achived or not.
        /// </summary>
        public bool finished { get; set; }
        /// <summary>
        /// GoalType is the Enum varible above.
        /// </summary>
        public GoalType goalType { get; set; }
        /// <summary>
        /// The ID of the user that created the Goal
        /// </summary>
        public string AspNetUserID { get; set; }
        /// <summary>
        /// The ID of the Project that has the Goal.
        /// </summary>
        public int ProjectID { get; set; }
    //    public virtual AspNetUser user { get; set; }
    //    public virtual Project project { get; set; }
    }
}