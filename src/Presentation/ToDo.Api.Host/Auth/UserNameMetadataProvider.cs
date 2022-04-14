using System.Collections.Generic;
using System.Security.Claims;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.EventStores;
using Microsoft.AspNetCore.Http;

namespace ToDo.Api.Host.Auth
{
    internal class UserNameMetadataProvider : IMetadataProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserNameMetadataProviderOptions _options;

        public UserNameMetadataProvider(IHttpContextAccessor httpContextAccessor, UserNameMetadataProviderOptions options)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options;
        }

        public IEnumerable<KeyValuePair<string, string>> ProvideMetadata<TAggregate, TIdentity>(TIdentity id, IAggregateEvent aggregateEvent, IMetadata metadata) where TAggregate : IAggregateRoot<TIdentity> where TIdentity : IIdentity
        {
            if (_httpContextAccessor.HttpContext is {User: { } user })
            {
                yield return new KeyValuePair<string, string>("username", user.FindFirstValue(_options.UserNameClaimType));
            }
        }
    }
}