using System.Threading.Tasks;
using SN.Api.Common;
using ToDo.Api.Contract.ToDo.Models;

namespace ToDo.Api.Contract.ToDo
{
    /// <summary>
    /// ToDo Service
    /// </summary>
    public interface IToDo : IApiContract
    {
        /// <summary>
        /// Get all lists
        /// </summary>
        /// <returns></returns>
        Task<ToDoListCollection> GetAllLists();
        
        /// <summary>
        /// Get list by id
        /// </summary>
        Task<ToDoList> GetList(string id);
        
        /// <summary>
        /// Create new list
        /// </summary>
        /// <param name="name">The list name</param>
        /// <returns></returns>
        Task<ToDoList> NewList(string name);
        
        /// <summary>
        /// Add n task to an existing list 
        /// </summary>
        /// <param name="listId">The list id in which the task will be added</param>
        /// <param name="task">The task to execute</param>
        /// <returns></returns>
        Task<ToDoItem> NewTask(string listId, string task);

        /// <summary>
        /// Get all tasks for specified list
        /// </summary>
        /// <param name="listId">The list id from witch to fetch tasks</param>
        /// <returns></returns>
        Task<ToDoItemCollection> GetTasks(string listId);

        /// <summary>
        /// Update the name of the list
        /// </summary>
        Task UpdateListName(string id, string name);

        /// <summary>
        /// Mark task as completed
        /// </summary>
        /// <param name="id">The task ID</param>
        Task TaskDone(string id);
    }
}