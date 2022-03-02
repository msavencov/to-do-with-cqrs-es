using MS.RestApi.Abstractions;
using ToDo.Api.Contract.Common;

namespace ToDo.Api.Contract.List.Operations
{
    /// <summary>
    /// Create a new list
    /// </summary>
    [EndPoint(Method.Post, "list/create", "List")]
    public class CreateList : MediatorRequest<Models.ToDoList>
    {
        /// <summary>
        /// The list name
        /// </summary>
        public string Name { get; set; }
    }
}