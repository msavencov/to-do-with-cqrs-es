using System.Collections.Generic;
using EventFlow.Queries;
using ToDo.Core.Item;
using ToDo.Core.List;

namespace ToDo.ReadStore.Items.Queries
{
    public class GetTasksQuery : IQuery<IEnumerable<ToDoItemReadModel>>
    {
        public ToDoItemId Id { get; init; }
        public ToDoListId ListId { get; init; }
        public bool IncludeDeleted { get; set; }
        public string TitleContains { get; set; }
    }
}