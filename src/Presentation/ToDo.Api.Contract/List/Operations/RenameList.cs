using MS.RestApi.Abstractions;
using ToDo.Api.Contract.Common;

namespace ToDo.Api.Contract.List.Operations
{
    /// <summary>
    /// Rename the list
    /// </summary>
    [EndPoint(Method.Post, "list/rename", "List")]
    public class RenameList : MediatorRequest
    {
        /// <summary>
        /// The list identifier to be renamed
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The new list name
        /// </summary>
        public string Name { get; set; }
    }
}