using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using ToDo.Core.Item;
using ToDo.Core.Item.Events;

namespace ToDo.ReadStore.ToDo
{
    public class ToDoItemReadModel : IReadModel
        , IAmAsyncReadModelFor<ToDoItem, ToDoItemId, ToDoItemCreated>
        , IAmAsyncReadModelFor<ToDoItem, ToDoItemId, ToDoItemCompleted>
        , IAmAsyncReadModelFor<ToDoItem, ToDoItemId, ToDoItemDeleted>
        , IAmAsyncReadModelFor<ToDoItem, ToDoItemId, ToDoItemRenamed>
    {
        public string Id { get; set; }
        public string ListId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        
        public Task ApplyAsync(IReadModelContext context, IDomainEvent<ToDoItem, ToDoItemId, ToDoItemCreated> domainEvent, CancellationToken cancellationToken)
        {
            Id = domainEvent.AggregateIdentity.Value;
            ListId = domainEvent.AggregateEvent.ListId.Value;
            Description = domainEvent.AggregateEvent.Task;
            CreatedAt = domainEvent.Timestamp;
            CreatedBy = domainEvent.Metadata.GetValueOrDefault(KnownMetadataKeys.UserName);

            return Task.CompletedTask;
        }

        public Task ApplyAsync(IReadModelContext context, IDomainEvent<ToDoItem, ToDoItemId, ToDoItemCompleted> domainEvent, CancellationToken cancellationToken)
        {
            IsCompleted = true;
            
            return Task.CompletedTask;
        }
        
        public Task ApplyAsync(IReadModelContext context, IDomainEvent<ToDoItem, ToDoItemId, ToDoItemDeleted> domainEvent, CancellationToken cancellationToken)
        {
            IsDeleted = true;
            
            return Task.CompletedTask;
        }

        public Task ApplyAsync(IReadModelContext context, IDomainEvent<ToDoItem, ToDoItemId, ToDoItemRenamed> domainEvent, CancellationToken cancellationToken)
        {
            Description = domainEvent.AggregateEvent.NewName;
            
            return Task.CompletedTask;
        }
    }
}


