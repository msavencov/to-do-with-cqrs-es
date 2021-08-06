using EventFlow.Aggregates;

namespace ToDo.Core.List.Events
{
    public class ListNameChanged : AggregateEvent<ToDoList, ToDoListId>
    {
        public string Name { get; }

        public ListNameChanged(string name)
        {
            Name = name;
        }
    }
}