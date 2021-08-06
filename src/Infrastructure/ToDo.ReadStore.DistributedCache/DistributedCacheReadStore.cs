using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Microsoft.Extensions.Caching.Distributed;

namespace ToDo.ReadStore.DistributedCache
{
    internal class DistributedCacheReadStore<TReadModel> : IDistributedCacheReadStore<TReadModel> 
        where TReadModel : class, IReadModel
    {
        private readonly IDistributedCache _cache;
        
        public DistributedCacheReadStore(IDistributedCache cache)
        {
            _cache = cache;
        }
        
        public async Task DeleteAsync(string id, CancellationToken ct)
        {
            await _cache.RemoveAsync(id, ct);
        }

        public Task DeleteAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ReadModelEnvelope<TReadModel>> GetAsync(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IReadOnlyCollection<ReadModelUpdate> readModelUpdates, IReadModelContextFactory readModelContextFactory,
            Func<IReadModelContext, IReadOnlyCollection<IDomainEvent>, ReadModelEnvelope<TReadModel>, CancellationToken, Task<ReadModelUpdateResult<TReadModel>>> updateReadModel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}