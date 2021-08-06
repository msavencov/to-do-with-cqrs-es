using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ToDo.ReadStore.DistributedCache.Extensions
{
    internal static class DistributedCacheExtensions
    {
        public static async Task<TModel> GetModelAsync<TModel>(this IDistributedCache cache, string key, CancellationToken ct = default)
        {
            var payload = await cache.GetStringAsync(key, ct);

            if (payload == null)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<TModel>(null);
        }

        public static async Task SetModelAsync<TModel>(this IDistributedCache cache, string key, TModel model, CancellationToken ct = default)
        {
            await cache.SetStringAsync(key, JsonConvert.SerializeObject(model), ct);
        }
    }
}