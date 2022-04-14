using ToDo.Api.Client.Infrastructure;

namespace DummyDataGenerator.Api
{
    public class AccessTokenAccessor : IAccessTokenAccessor
    {
        public string AccessToken { get; set; }
    }
}