using System;
using System.Collections.Generic;
using EventFlow.ValueObjects;

namespace ToDo.ReadStore.DistributedCache.ValueObjects
{
    public class ReadModelDescription : ValueObject
    {
        public ReadModelDescription(IndexName indexName)
        {
            IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
        }

        public IndexName IndexName { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return IndexName;
        }
    }
}