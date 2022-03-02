using System;

namespace ToDo.Api.Contract.List.Models
{
    /// 
    public class ToDoList
    {
        /// <summary>
        /// The List identifier 
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The list name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The date when list was created on
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }
        
        /// <summary>
        /// The username who created the task
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// The qty of tasks in the list
        /// </summary>
        public int TaskCount { get; set; }

        /// <summary>
        /// The qty of tasks to be done
        /// </summary>
        public int ActiveTaskCount { get; set; }
    }
}