using EventFlow.Commands;

namespace ToDo.Core.Item.Commands
{
    public class RenameToDoItem : Command<ToDoItem, ToDoItemId>
    {
        public ToDoItemId ItemId { get; }
        public string Name { get; }

        public RenameToDoItem(ToDoItemId itemId, string name) : base(itemId)
        {
            ItemId = itemId;
            Name = name;
        }
    }
}