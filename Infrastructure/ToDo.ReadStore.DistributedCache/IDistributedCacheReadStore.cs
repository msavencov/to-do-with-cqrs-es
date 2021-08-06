using EventFlow.ReadStores;

namespace ToDo.ReadStore.DistributedCache
{
    public interface IDistributedCacheReadStore<TReadModel> : IReadModelStore<TReadModel> 
        where TReadModel : class, IReadModel
    {
    }
}