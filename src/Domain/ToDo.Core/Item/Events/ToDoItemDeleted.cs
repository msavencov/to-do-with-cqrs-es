using EventFlow.Aggregates;
using ToDo.Core.List;

namespace ToDo.Core.Item.Events
{
    public class ToDoItemDeleted : AggregateEvent<ToDoItem, ToDoItemId>
    {
        public ToDoItemDeleted(ToDoItemId id, ToDoListId listId)
        {
            Id = id;
            ListId = listId;
        }
        
        public ToDoItemId Id { get; }
        public ToDoListId ListId { get; }
    }
}