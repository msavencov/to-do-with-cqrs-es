using System.Net.Http;
using SN.Api.Client.Implementation;
using SN.Api.Client.Internal;

namespace DummyDataGenerator.Data
{
    internal class ToDoApiProxyFactory : DynamicProxyFactory<ApiDynamicProxy>
    {
        private readonly HttpClient _client;

        public ToDoApiProxyFactory(HttpClient client) : base(new DynamicInterfaceImplementor())
        {
            _client = client;
        }

        public override TInterfaceType CreateDynamicProxy<TInterfaceType>(params object[] constructorParameters)
        {
            return base.CreateDynamicProxy<TInterfaceType>(_client);
        }
    }

}