using System.Collections.Generic;

namespace ToDo.Api.Contract.ToDo.Models
{
    /// <summary>
    /// The container for task items  
    /// </summary>
    public class ToDoItemCollection : List<ToDoItem>
    {
        /// <inheritdoc />
        public ToDoItemCollection() 
        {
        }

        /// <inheritdoc />
        public ToDoItemCollection(IEnumerable<ToDoItem> collection) : base(collection)
        {
            
        }
    }
}