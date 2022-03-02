using System.ComponentModel.DataAnnotations;
using MS.RestApi.Abstractions;
using ToDo.Api.Contract.Common;

namespace ToDo.Api.Contract.Tasks.Operations
{
    /// <summary>
    /// Change the name of the task
    /// </summary>
    [EndPoint(Method.Post, "task/rename", "ToDo")]
    public class RenameTask : MediatorRequest
    {
        /// <summary>
        /// The task identifier
        /// </summary>
        [Required]
        public string TaskId { get; set; }
        
        /// <summary>
        /// New task name
        /// </summary>
        [Required, MinLength(10)]
        public string NewName { get; set; }
    }
}