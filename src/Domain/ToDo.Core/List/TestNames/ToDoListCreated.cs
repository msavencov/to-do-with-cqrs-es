using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ToDo.Core.List.TestNames
{
    [EventVersion("List.ToDoListCreated", 1)]
    public class ToDoListCreated : AggregateEvent<ToDoList, ToDoListId>
    {
        public ToDoListCreated(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}