using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Logs;
using EventFlow.ReadStores;
using EventFlow.ReadStores.InMemory;

namespace ToDo.ReadStore.Abstractions.ReadStores
{
    public class InMemorySearchableReadStore<TReadModel> : InMemoryReadStore<TReadModel>, ISearchableReadModelStore<TReadModel> where TReadModel : class, IReadModel, new()
    {
        public InMemorySearchableReadStore(ILog log) : base(log)
        {
        }

        public Task<IReadOnlyCollection<TReadModel>> FindAsync(Expression<Func<TReadModel, bool>> predicate,
            CancellationToken ct = default)
        {
            var func = predicate.Compile();
            return base.FindAsync(t => func(t), ct);
        }
    }
}