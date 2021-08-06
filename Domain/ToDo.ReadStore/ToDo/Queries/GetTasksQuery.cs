using System.Collections.Generic;
using EventFlow.Queries;
using ToDo.Core.List;

namespace ToDo.ReadStore.ToDo.Queries
{
    public class GetTasksQuery : IQuery<IEnumerable<ToDoItemReadModel>>
    {
        public ToDoListId ListId { get; }

        public GetTasksQuery(ToDoListId listId)
        {
            ListId = listId;
        }
    }
}