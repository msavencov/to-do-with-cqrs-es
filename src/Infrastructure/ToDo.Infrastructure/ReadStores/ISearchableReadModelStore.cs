using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.ReadStores;

namespace ToDo.Infrastructure.ReadStores
{
    public interface ISearchableReadModelStore<TReadModel> : IReadModelStore<TReadModel> where TReadModel : class, IReadModel, new()
    {
        Task<IReadOnlyCollection<TReadModel>> FindAsync(Expression<Func<TReadModel, bool>> predicate, CancellationToken ct = default);
    }
}