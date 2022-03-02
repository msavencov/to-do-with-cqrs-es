using MS.RestApi.Abstractions;
using ToDo.Api.Contract.Common;

namespace ToDo.Api.Contract.Tasks.Operations
{
    /// <summary>
    /// Mark task as completed
    /// </summary>
    [EndPoint(Method.Post, "task/complete", "ToDo")]
    public class CompleteTask : MediatorRequest
    {
        /// <summary>
        /// The task identifier
        /// </summary>
        public string TaskId { get; set; }
    }
}