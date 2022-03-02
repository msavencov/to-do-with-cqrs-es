using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MS.RestApi.Client;
using ToDo.Api.Client.ToDoApi;

namespace ToDo.Api.Client.Infrastructure
{
    internal class ToDoApiRequestHandler : RequestHandlerBase, IToDoApiRequestHandler
    {
        public ToDoApiRequestHandler(HttpClient client, IAccessTokenAccessor accessTokenAccessor) : base(client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenAccessor.AccessToken);
        }

        protected override Task OnRequestMessageSentAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                
            }
            
            return base.OnRequestMessageSentAsync(response);
        }
    }
}