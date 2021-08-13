using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using ToDo.Core.Item;
using ToDo.Core.Item.Events;
using ToDo.Core.List;
using ToDo.Core.List.Events;

namespace ToDo.ReadStore.ToDo
{
    public class ToDoListReadModelLocator : IReadModelLocator
    {
        public IEnumerable<string> GetReadModelIds(IDomainEvent domainEvent)
        {
            if (domainEvent is IDomainEvent<ToDoList, ToDoListId, ToDoListCreated>)
            {
                yield return domainEvent.GetIdentity().Value;
            }
            
            if (domainEvent is IDomainEvent<ToDoItem, ToDoItemId, ToDoItemCreated> itemCreated)
            {
                yield return itemCreated.AggregateEvent.ListId.Value;
            }

            if (domainEvent is IDomainEvent<ToDoItem, ToDoItemId, ToDoItemCompleted> itemCompleted)
            {
                yield return itemCompleted.AggregateEvent.ListId.Value;
            }
            
            if (domainEvent is IDomainEvent<ToDoItem, ToDoItemId, ToDoItemCompleted> itemDeleted)
            {
                yield return itemDeleted.AggregateEvent.ListId.Value;
            }
                
        }
    }
}