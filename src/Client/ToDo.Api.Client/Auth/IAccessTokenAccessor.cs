namespace ToDo.Api.Client.Auth
{
    public interface IAccessTokenAccessor
    {
        public string AccessToken { get; set; }
    }
}