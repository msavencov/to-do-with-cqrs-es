using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.EntityFramework;
using EventFlow.EntityFramework.ReadStores;
using EventFlow.ReadStores;
using EventFlow.Specifications;
using Microsoft.EntityFrameworkCore;
using ToDo.ReadStore.Abstractions.Shared;
using ToDo.ReadStore.Abstractions.Shared.Specifications;

namespace ToDo.ReadStore.Abstractions.ReadStores
{
    public class EfSearchableReadStore<TReadModel, TDbContext> : ISearchableReadModelStore<TReadModel>
        where TReadModel : class, IReadModel, new() where TDbContext : DbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;
        private readonly IEntityFrameworkReadModelStore<TReadModel> _readStore;

        public EfSearchableReadStore(IEntityFrameworkReadModelStore<TReadModel> readStore,
            IDbContextProvider<TDbContext> dbContextProvider)
        {
            _readStore = readStore;
            _dbContextProvider = dbContextProvider;
        }

        public async Task<IReadOnlyCollection<TReadModel>> FindAsync(Expression<Func<TReadModel, bool>> predicate,
            CancellationToken ct = default)
        {
            var spec = new ReadModelSpec<TReadModel>().WithCriteria(predicate);
            var result = await FindAsync(spec, ct);

            return result.Result;
        }

        public async Task<PagedResult<TReadModel>> FindAsync(ReadModelSpec<TReadModel> specification, CancellationToken ct)
        {
            using (var dbContext = _dbContextProvider.CreateContext())
            {
                var set = dbContext.Set<TReadModel>().AsNoTracking().AsQueryable();
                var result = await set.ApplySpecification(specification, ct);
                var rows = await result.Result.ToListAsync(ct);

                return new PagedResult<TReadModel>(result.TotalRows, rows);
            }
        }

        public Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            return _readStore.DeleteAsync(id, cancellationToken);
        }

        public Task DeleteAllAsync(CancellationToken cancellationToken)
        {
            return _readStore.DeleteAllAsync(cancellationToken);
        }

        public Task<ReadModelEnvelope<TReadModel>> GetAsync(string id, CancellationToken cancellationToken)
        {
            return _readStore.GetAsync(id, cancellationToken);
        }

        public Task UpdateAsync(IReadOnlyCollection<ReadModelUpdate> readModelUpdates,
            IReadModelContextFactory readModelContextFactory,
            Func<IReadModelContext, IReadOnlyCollection<IDomainEvent>, ReadModelEnvelope<TReadModel>, CancellationToken,
                Task<ReadModelUpdateResult<TReadModel>>> updateReadModel, CancellationToken cancellationToken)
        {
            return _readStore.UpdateAsync(readModelUpdates, readModelContextFactory, updateReadModel,
                cancellationToken);
        }
    }
}