using MS.RestApi.Abstractions;
using ToDo.Api.Contract.Common;
using ToDo.Api.Contract.List.Models;

namespace ToDo.Api.Contract.List.Operations
{
    /// <summary>
    /// List all projects
    /// </summary>
    [EndPoint(Method.Get, "list/find", "List")]
    public class FindLists : MediatorRequest<ToDoListCollection>
    {
        
    }
}