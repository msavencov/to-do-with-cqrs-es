using Grpc.Core;
using Grpc.Core.Interceptors;

namespace ToDo.Api.Client.Auth;

internal class AuthInterceptor : Interceptor
{
    private readonly IAccessTokenAccessor _tokenProvider;
    
    public AuthInterceptor(IAccessTokenAccessor tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }
    
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var headers = new Metadata()
        {
            new("Authorization", $"Bearer {_tokenProvider.AccessToken}")
        };
        var newOptions = context.Options.WithHeaders(headers);
        var newContext = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, newOptions);

        return base.AsyncUnaryCall(request, newContext, continuation);
    }
}