using EventFlow;
using EventFlow.Extensions;
using EventFlow.ReadStores;

namespace ToDo.ReadStore.DistributedCache.Extensions
{
    public static class EventFlowOptionsExtensions
    {
        public static IEventFlowOptions UseDistributedCacheReadModel<TReadModel, TReadModelLocator>(this IEventFlowOptions eventFlowOptions) 
            where TReadModel : class, IReadModel
            where TReadModelLocator : IReadModelLocator
        {
            eventFlowOptions.RegisterServices(registration =>
            {
                registration.Register<IDistributedCacheReadStore<TReadModel>, DistributedCacheReadStore<TReadModel>>();
                registration.Register<IReadModelStore<TReadModel>>(r => r.Resolver.Resolve<IDistributedCacheReadStore<TReadModel>>());
            });
            eventFlowOptions.UseReadStoreFor<IDistributedCacheReadStore<TReadModel>, TReadModel, TReadModelLocator>();

            return eventFlowOptions;
        }
    }
}