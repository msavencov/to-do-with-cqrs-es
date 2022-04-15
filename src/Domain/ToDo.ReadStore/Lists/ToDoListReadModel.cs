using System;
using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using ToDo.Core.Item;
using ToDo.Core.Item.Events;
using ToDo.Core.List;
using ToDo.Core.List.Events;

namespace ToDo.ReadStore.Lists
{
    public class ToDoListReadModel : IReadModel
        , IAmReadModelFor<ToDoList, ToDoListId, ToDoListCreated>
        , IAmReadModelFor<ToDoList, ToDoListId, ListNameChanged>
        , IAmReadModelFor<ToDoItem, ToDoItemId, ToDoItemCreated>
        , IAmReadModelFor<ToDoItem, ToDoItemId, ToDoItemCompleted>
        , IAmReadModelFor<ToDoItem, ToDoItemId, ToDoItemDeleted>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public int TaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
            
        public void Apply(IReadModelContext context, IDomainEvent<ToDoList, ToDoListId, ToDoListCreated> domainEvent)
        {
            Id = domainEvent.AggregateIdentity.Value;
            Name = domainEvent.AggregateEvent.Name;
            CreatedAt = domainEvent.Timestamp;
            CreatedBy = domainEvent.Metadata.GetValueOrDefault(KnownMetadataKeys.UserName, null);
        }

        public void Apply(IReadModelContext context, IDomainEvent<ToDoItem, ToDoItemId, ToDoItemCreated> domainEvent)
        {
            TaskCount += 1;
        } 
        
        public void Apply(IReadModelContext context, IDomainEvent<ToDoItem, ToDoItemId, ToDoItemCompleted> domainEvent)
        {
            CompletedTaskCount += 1;
        } 
        public void Apply(IReadModelContext context, IDomainEvent<ToDoItem, ToDoItemId, ToDoItemDeleted> domainEvent)
        {
            TaskCount -= 1;
        }

        public void Apply(IReadModelContext context, IDomainEvent<ToDoList, ToDoListId, ListNameChanged> domainEvent)
        {
            Name = domainEvent.AggregateEvent.Name;
        }
    }
}