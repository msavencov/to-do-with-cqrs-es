using ToDo.Api.Client.Infrastructure;

namespace ToDo.App.Auth
{
    public class AccessTokenAccessor : IAccessTokenAccessor
    {
        private readonly TokenProvider _provider;

        public AccessTokenAccessor(TokenProvider provider)
        {
            _provider = provider;
        }
        
        public string AccessToken { get => _provider.AccessToken; set => _ = value; }
    }
}