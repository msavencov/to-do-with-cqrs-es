using MS.RestApi.Abstractions;
using ToDo.Api.Contract.Common;

namespace ToDo.Api.Contract.Tasks.Operations
{
    /// <summary>
    /// Completely delete a task
    /// </summary>
    [EndPoint(Method.Delete, "task", "ToDo")]
    public class DeleteTask : MediatorRequest
    {
        /// <summary>
        /// The task identifier
        /// </summary>
        public string TaskId { get; set; }
    }
}