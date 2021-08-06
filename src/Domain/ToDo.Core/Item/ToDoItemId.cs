using EventFlow.Core;

namespace ToDo.Core.Item
{
    public class ToDoItemId : Identity<ToDoItemId>
    {
        public ToDoItemId(string value) : base(value)
        {
        }
    }
}