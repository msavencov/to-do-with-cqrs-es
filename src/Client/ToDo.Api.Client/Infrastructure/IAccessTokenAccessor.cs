namespace ToDo.Api.Client.Infrastructure
{
    public interface IAccessTokenAccessor
    {
        public string AccessToken { get; set; }
    }
}