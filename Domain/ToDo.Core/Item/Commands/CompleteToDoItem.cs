using EventFlow.Commands;

namespace ToDo.Core.Item.Commands
{
    public class CompleteToDoItem : Command<ToDoItem, ToDoItemId>
    {
        public CompleteToDoItem(ToDoItemId aggregateId) : base(aggregateId)
        {
        }
    }
}