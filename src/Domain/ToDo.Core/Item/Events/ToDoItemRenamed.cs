using EventFlow.Aggregates;

namespace ToDo.Core.Item.Events
{
    public class ToDoItemRenamed : AggregateEvent<ToDoItem, ToDoItemId>
    {
        public ToDoItemId Id { get; }
        public string NewName { get; }

        public ToDoItemRenamed(ToDoItemId id, string name)
        {
            Id = id;
            NewName = name;
        }
    }
}