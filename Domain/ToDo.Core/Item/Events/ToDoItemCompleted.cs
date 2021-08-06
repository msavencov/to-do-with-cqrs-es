using EventFlow.Aggregates;
using ToDo.Core.List;

namespace ToDo.Core.Item.Events
{
    public class ToDoItemCompleted : AggregateEvent<ToDoItem, ToDoItemId>
    {
        public ToDoItemCompleted(ToDoItemId id, ToDoListId listId)
        {
            Id = id;
            ListId = listId;
        }

        public ToDoItemId Id { get; }
        public ToDoListId ListId { get; }
    }
}