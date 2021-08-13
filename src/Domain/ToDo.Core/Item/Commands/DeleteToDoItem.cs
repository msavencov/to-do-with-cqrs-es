using EventFlow.Commands;

namespace ToDo.Core.Item.Commands
{
    public class DeleteToDoItem : Command<ToDoItem, ToDoItemId>
    {
        public DeleteToDoItem(ToDoItemId aggregateId) : base(aggregateId)
        {
        }
    }
}