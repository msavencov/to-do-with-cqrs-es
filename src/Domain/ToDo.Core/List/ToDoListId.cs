using EventFlow.Core;

namespace ToDo.Core.List
{
    public class ToDoListId : Identity<ToDoListId>
    {
        public ToDoListId(string value) : base(value)
        {
        }
    }
}