using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using ToDo.Infrastructure.ReadStores;
using ToDo.ReadStore.ToDo;
using ToDo.ReadStore.ToDo.Queries;

namespace ToDo.ReadStore.EF.Handlers
{
    public class GetAllListsQueryHandler : IQueryHandler<GetAllListsQuery, IEnumerable<ToDoListReadModel>>
    {
        private readonly ISearchableReadModelStore<ToDoListReadModel> _modelStore;

        public GetAllListsQueryHandler(ISearchableReadModelStore<ToDoListReadModel> modelStore)
        {
            _modelStore = modelStore;
        }

        public async Task<IEnumerable<ToDoListReadModel>> ExecuteQueryAsync(GetAllListsQuery query,
            CancellationToken ct)
        {
            return (await _modelStore.FindAsync(t => true, ct)).OrderByDescending(t => t.CreatedAt);
        }
    }

    class GetTasksQueryHandler : IQueryHandler<GetTasksQuery, IEnumerable<ToDoItemReadModel>>
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
}