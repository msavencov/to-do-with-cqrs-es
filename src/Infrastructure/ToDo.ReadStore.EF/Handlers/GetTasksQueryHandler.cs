using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using ToDo.ReadStore.Abstractions.ReadStores;
using ToDo.ReadStore.Extensions;
using ToDo.ReadStore.Items;
using ToDo.ReadStore.Items.Queries;

namespace ToDo.ReadStore.EF.Handlers;

internal class GetTasksQueryHandler : IQueryHandler<GetTasksQuery, IEnumerable<ToDoItemReadModel>>
{
    private readonly ISearchableReadModelStore<ToDoItemReadModel> _modelStore;

    public GetTasksQueryHandler(ISearchableReadModelStore<ToDoItemReadModel> modelStore)
    {
        _modelStore = modelStore;
    }

    public async Task<IEnumerable<ToDoItemReadModel>> ExecuteQueryAsync(GetTasksQuery query, CancellationToken ct)
    {
        var filter = PredicateBuilder.True<ToDoItemReadModel>();
        
        if (query.Id is {Value.Length: > 0})
        {
            filter = filter.And(t => t.Id == query.Id.Value);
        }

        if (query.ListId is {Value.Length: > 0})
        {
            filter = filter.And(t => t.ListId == query.ListId.Value);
        }

        if (query.IncludeDeleted is false)
        {
            filter = filter.And(t => t.IsDeleted == false);
        }

        if (query.TitleContains is {Length: > 0})
        {
            filter = filter.And(t => t.Description.Contains(query.TitleContains));
        }

        return await _modelStore.FindAsync(filter, ct);
    }
}