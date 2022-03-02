using System.ComponentModel.DataAnnotations;
using MS.RestApi.Abstractions;
using ToDo.Api.Contract.Common;
using ToDo.Api.Contract.Tasks.Models;

namespace ToDo.Api.Contract.List.Operations
{
    /// <summary>
    /// Get all tasks for specified list
    /// </summary>
    [EndPoint(Method.Get, "list/tasks", "List")]
    public class GetTasks : MediatorRequest<ToDoItemCollection>
    {
        /// <summary>
        /// The list id from witch to fetch tasks
        /// </summary>
        [Required]
        public string ListId { get; set; }
    }
}