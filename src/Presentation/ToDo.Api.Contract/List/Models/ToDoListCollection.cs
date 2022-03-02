using System.Collections.Generic;

namespace ToDo.Api.Contract.List.Models
{
    /// <summary>
    /// The collection of list 
    /// </summary>
    public class ToDoListCollection : List<ToDoList>
    {
        /// <inheritdoc />
        public ToDoListCollection(IEnumerable<ToDoList> inner) : base(inner)
        {
        }

        /// <inheritdoc />
        public ToDoListCollection()
        {
            
        }
    }
}