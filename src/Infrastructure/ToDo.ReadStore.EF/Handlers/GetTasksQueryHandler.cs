using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using ToDo.Infrastructure.ReadStores;
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
        return (
                await _modelStore.FindAsync(t => t.ListId == query.ListId.Value, ct)
            )
            .OrderByDescending(t => t.CreatedAt);
    }
}