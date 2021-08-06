using EventFlow.Aggregates;
using ToDo.Core.List.Events;

namespace ToDo.Core.List
{
    public class ToDoList : AggregateRoot<ToDoList, ToDoListId>
        , IEmit<ToDoListCreated>
    {
        private string _name;
        
        public ToDoList(ToDoListId listId) : base(listId)
        {
            
        }

        public void Create(string name)
        {
            Emit(new ToDoListCreated(name));
        }
        
        public void Apply(ToDoListCreated aggregateEvent)
        {
            _name = aggregateEvent.Name;
        }

        public void ChangeName(string name)
        {
            Emit(new ListNameChanged(name));
        }
    }
}