using System;
using System.Collections.Concurrent;
using EventFlow.Extensions;
using EventFlow.ReadStores;
using ToDo.ReadStore.DistributedCache.ValueObjects;

namespace ToDo.ReadStore.DistributedCache.ReadStores
{
    public class ReadModelDescriptionProvider : IReadModelDescriptionProvider
    {
        private static readonly ConcurrentDictionary<Type, ReadModelDescription> IndexNames = new();

        public ReadModelDescription GetReadModelDescription<TReadModel>() where TReadModel : IReadModel
        {
            return IndexNames.GetOrAdd(typeof(TReadModel), t =>
            {
                var modelTypeName = t.PrettyPrint().ToLowerInvariant();
                var indexName = new IndexName($"eventflow-{modelTypeName}");

                return new ReadModelDescription(indexName);
            });
        }
    }
}