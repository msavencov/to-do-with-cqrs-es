using EventFlow.Aggregates;

namespace ToDo.Core.List.Events
{
    public class ToDoListCreated : AggregateEvent<ToDoList, ToDoListId>
    {
        public ToDoListCreated(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
    
    
}