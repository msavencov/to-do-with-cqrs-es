using System.ComponentModel.DataAnnotations;
using MS.RestApi.Abstractions;
using ToDo.Api.Contract.Common;
using ToDo.Api.Contract.Tasks.Models;

namespace ToDo.Api.Contract.Tasks.Operations
{
    /// <summary>
    /// Add new task to an existing list 
    /// </summary>
    [EndPoint(Method.Post, "task", "ToDo")]
    public class AddTask : MediatorRequest<ToDoItem>
    {
        /// <summary>
        /// The list id in which the task will be created
        /// </summary>
        [Required]
        public string ListId { get; set; }
        
        /// <summary>
        /// The task to execute
        /// </summary>
        [Required, MinLength(10)]
        public string Task { get; set; }
    }
}