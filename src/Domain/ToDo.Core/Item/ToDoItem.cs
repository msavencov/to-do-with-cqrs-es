using EventFlow.Aggregates;
using EventFlow.Exceptions;
using ToDo.Core.Item.Events;
using ToDo.Core.List;

namespace ToDo.Core.Item
{
    public class ToDoItem : AggregateRoot<ToDoItem, ToDoItemId>
    {
        private string _task;
        private TaskItemState _state = TaskItemState.Open;
        private ToDoListId _listId;

        public ToDoItem(ToDoItemId id) : base(id)
        {
            
        }
        
        public void Create(ToDoListId toDoListId, string task)
        {
            Emit(new ToDoItemCreated(toDoListId, task));
        }

        internal void Apply(ToDoItemCreated aggregateEvent)
        {
            _listId = aggregateEvent.ListId;
            _task = aggregateEvent.Task;
        }

        public void Complete()
        {
            if (_state == TaskItemState.Completed)
            {
                throw DomainError.With("The task is already completed.");
            }
            
            Emit(new ToDoItemCompleted(Id, _listId));
        }

        internal void Apply(ToDoItemCompleted _)
        {
            _state = TaskItemState.Completed;
        }
    }
}