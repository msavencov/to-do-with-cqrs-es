using System;

namespace ToDo.Api.Contract.Tasks.Models
{
    /// 
    public class ToDoItem
    {
        /// <summary>
        /// The task identifier
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The list identifier 
        /// </summary>
        public string ListId { get; set; }
        
        /// <summary>
        /// The task description
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// The date when the task was created on
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The username who created the task
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// The task is Completed
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}