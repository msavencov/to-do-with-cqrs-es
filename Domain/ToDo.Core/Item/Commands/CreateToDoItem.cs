using EventFlow.Commands;
using ToDo.Core.List;

namespace ToDo.Core.Item.Commands
{
    public class CreateToDoItem : Command<ToDoItem, ToDoItemId>
    {
        public ToDoListId ListId { get; }
        public string Task { get; }

        public CreateToDoItem(ToDoListId listId, string task) : base(ToDoItemId.New)
        {
            ListId = listId;
            Task = task;
        }
    }
}