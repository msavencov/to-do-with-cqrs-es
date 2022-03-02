using MS.RestApi.Abstractions;
using ToDo.Api.Contract.Common;

namespace ToDo.Api.Contract.List.Operations
{
    /// <summary>
    /// Get list by id
    /// </summary>
    [EndPoint(Method.Get, "list/item", "List")]
    public class GetList : MediatorRequest<Models.ToDoList>
    {
        /// <summary>
        /// The list identifier
        /// </summary>
        public string ListId { get; set; }
    }
}