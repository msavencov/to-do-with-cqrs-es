using System;
using EventFlow.ValueObjects;

namespace ToDo.ReadStore.DistributedCache.ValueObjects
{
    public class IndexName : SingleValueObject<string>
    {
        public IndexName(string value) : base(value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
        }
    }
}