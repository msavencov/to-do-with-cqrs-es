using System.Net.Http;
using System.Net.Http.Headers;
using SN.Api.Client.Implementation;
using SN.Api.Client.Internal;
using ToDo.App.Auth;

namespace ToDo.App.Data
{
    internal class ToDoApiProxyFactory : DynamicProxyFactory<ApiDynamicProxy>
    {
        private readonly HttpClient _client;

        public ToDoApiProxyFactory(HttpClient client, TokenProvider tokenProvider) : base(new DynamicInterfaceImplementor())
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenProvider.AccessToken);
        }

        public override TInterfaceType CreateDynamicProxy<TInterfaceType>(params object[] constructorParameters)
        {
            return base.CreateDynamicProxy<TInterfaceType>(_client);
        }
    }

}