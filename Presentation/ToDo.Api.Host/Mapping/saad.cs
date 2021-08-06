using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;

namespace ToDo.Api.Host.Mapping
{
    public static class Program
    {
        public static async Task Main1(string[] args)
        {
            var settings = EventStoreClientSettings.Create("esdb://localhost:2113?tls=false");
            var client = new EventStoreClient(settings);

            var data = new
            {
                Id = 1,
                Name = "sadsad dasdad"
            };
            
            var @event = new EventData(
                Uuid.NewUuid(),
                "TestEvent",
                JsonSerializer.SerializeToUtf8Bytes(data)
            );

            await client.AppendToStreamAsync(
                "some-stream",
                StreamState.Any,
                new[] {@event},
                cancellationToken: CancellationToken.None
            );
        }
    }
}

