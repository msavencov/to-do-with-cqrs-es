using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using ToDo.ReadStore.Abstractions.ReadStores;
using ToDo.ReadStore.Abstractions.Shared.Specifications;
using ToDo.ReadStore.Extensions;
using ToDo.ReadStore.Lists;
using ToDo.ReadStore.Lists.Queries;

namespace ToDo.ReadStore.EF.Handlers;

internal class FilterListsQueryHandler : IQueryHandler<FilterListsQuery, FilterListsQuery.Result>
{
    private readonly ISearchableReadModelStore<ToDoListReadModel> _modelStore;

    public FilterListsQueryHandler(ISearchableReadModelStore<ToDoListReadModel> modelStore)
    {
        _modelStore = modelStore;
    }

    public async Task<FilterListsQuery.Result> ExecuteQueryAsync(FilterListsQuery query, CancellationToken ct)
    {
        var spec = new ReadModelSpec<ToDoListReadModel>();
        var criteria = PredicateBuilder.True<ToDoListReadModel>();

        if (query.Criteria is { Count: > 0 })
        {
            foreach (var item in query.Criteria)
            {
                criteria = criteria.And(item);
            }

            spec = spec.WithCriteria(criteria);
        }

        if (query.Paging is { } paging)
        {
            spec = spec.Paged(paging.Page, paging.Rows);
        }

        if (query.Sorting is { } sorting)
        {
            if (sorting.Count > 1)
            {
                throw new NotImplementedException("Multiple column sorting is not implemented.");
            }
            var item = sorting.Single();
            
            spec = spec.SortBy(item.Expression, item.Descending);
        }

        var result = await _modelStore.FindAsync(spec, ct);

        return new FilterListsQuery.Result(result.TotalRows, result.Result);
    }
}