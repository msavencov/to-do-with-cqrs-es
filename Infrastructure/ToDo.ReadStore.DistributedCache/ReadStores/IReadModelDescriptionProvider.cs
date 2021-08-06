using EventFlow.ReadStores;
using ToDo.ReadStore.DistributedCache.ValueObjects;

namespace ToDo.ReadStore.DistributedCache.ReadStores
{
    public interface IReadModelDescriptionProvider
    {
        ReadModelDescription GetReadModelDescription<TReadModel>() where TReadModel : IReadModel;
    }
}