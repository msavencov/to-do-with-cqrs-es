using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Queries;
using ToDo.ReadStore.Abstractions.ReadStores;
using ToDo.ReadStore.Lists;
using ToDo.ReadStore.Lists.Queries;

namespace ToDo.ReadStore.EF.Handlers
{
    internal class GetAllListsQueryHandler : IQueryHandler<GetAllListsQuery, IEnumerable<ToDoListReadModel>>
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
}