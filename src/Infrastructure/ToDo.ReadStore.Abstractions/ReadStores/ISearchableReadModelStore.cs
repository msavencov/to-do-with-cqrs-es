using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.ReadStores;
using ToDo.ReadStore.Abstractions.Shared;
using ToDo.ReadStore.Abstractions.Shared.Specifications;

namespace ToDo.ReadStore.Abstractions.ReadStores
{
    public interface ISearchableReadModelStore<TReadModel> : IReadModelStore<TReadModel> where TReadModel : class, IReadModel, new()
    {
        Task<IReadOnlyCollection<TReadModel>> FindAsync
        (
            Expression<Func<TReadModel, bool>> predicate,
            CancellationToken ct = default
        );

        Task<PagedResult<TReadModel>> FindAsync(ReadModelSpec<TReadModel> specification, CancellationToken ct);
    }
}