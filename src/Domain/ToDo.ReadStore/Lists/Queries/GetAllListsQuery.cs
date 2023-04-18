using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EventFlow.Queries;
using ToDo.ReadStore.Shared;

namespace ToDo.ReadStore.Lists.Queries
{
    public class GetAllListsQuery : IQuery<IEnumerable<ToDoListReadModel>>
    {
    }

    public class FilterListsQuery : IQuery<FilterListsQuery.Result>
    {
        public FilterSettingsCollection<ToDoListReadModel> Criteria { get; } = new();
        public SortingSettingsCollection<ToDoListReadModel> Sorting { get; } = new();
        public PagingSettings Paging { get; set; } = new();

        public class Result : PagedResult<ToDoListReadModel>
        {
            public Result(int totalRows, IReadOnlyCollection<ToDoListReadModel> rows)
            {
                TotalRows = totalRows;
                Result = rows;
            }
        }
    }
}