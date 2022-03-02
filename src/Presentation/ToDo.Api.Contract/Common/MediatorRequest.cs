using MediatR;
using MS.RestApi.Abstractions;

namespace ToDo.Api.Contract.Common
{
    /// <inheritdoc cref="MS.RestApi.Abstractions.Request" />
    public abstract class MediatorRequest : Request, IRequest 
    {
    }
    
    /// <inheritdoc cref="MS.RestApi.Abstractions.Request{TResponse}" />
    public abstract class MediatorRequest<TResponse> : Request<TResponse>, IRequest<TResponse> 
    {
    }
}