using System;
using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using ToDo.Core.Item;
using ToDo.Core.Item.Events;

namespace ToDo.ReadStore.ToDo
{
    public class ToDoItemReadModel : IReadModel
        , IAmReadModelFor<ToDoItem, ToDoItemId, ToDoItemCreated>
        , IAmReadModelFor<ToDoItem, ToDoItemId, ToDoItemCompleted>
    {
        public string Id { get; set; }
        public string ListId { get; set; }
        public string Task { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public bool IsCompleted { get; set; }
        
        public void Apply(IReadModelContext context, IDomainEvent<ToDoItem, ToDoItemId, ToDoItemCreated> domainEvent)
        {
            Id = domainEvent.AggregateIdentity.Value;
            ListId = domainEvent.AggregateEvent.ListId.Value;
            Task = domainEvent.AggregateEvent.Task;
            CreatedAt = domainEvent.Timestamp;
            CreatedBy = domainEvent.Metadata.GetValueOrDefault(KnownMetadataKeys.UserName);
        }

        public void Apply(IReadModelContext context, IDomainEvent<ToDoItem, ToDoItemId, ToDoItemCompleted> domainEvent)
        {
            IsCompleted = true;
        }
    }
}


