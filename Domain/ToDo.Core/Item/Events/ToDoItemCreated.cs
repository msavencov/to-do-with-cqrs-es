using EventFlow.Aggregates;
using ToDo.Core.List;

namespace ToDo.Core.Item.Events
{
    public class ToDoItemCreated : AggregateEvent<ToDoItem, ToDoItemId>
    {
        public ToDoItemCreated(ToDoListId listId, string task)
        {
            ListId = listId;
            Task = task;
        }

        public ToDoListId ListId { get; }
        public string Task { get; }
    }
}