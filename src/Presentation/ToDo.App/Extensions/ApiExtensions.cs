using System;
using System.Threading.Tasks;

namespace ToDo.App.Extensions
{
    public static class ApiExtensions
    {
        public static async Task<TResult> CallSafeAsync<TApi, TResult>(this TApi api, Func<TApi, Task<TResult>> callback, Action<Exception> onError)
        {
            try
            {
                return await callback(api);
            }
            catch (Exception e)
            {
                onError(e);
            }

            return default;
        }
    }
}