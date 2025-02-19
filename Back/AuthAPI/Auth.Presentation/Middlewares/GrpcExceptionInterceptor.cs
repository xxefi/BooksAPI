using System.Text.Json;
using Auth.Application.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Auth.Presentation.Middlewares;

public class GrpcExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (AuthException ex) 
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (RpcException ex)
        {
            throw new RpcException(new Status(ex.StatusCode, ex.Status.Detail));
        }
        catch (Exception ex) 
        {
            throw new RpcException(new Status(StatusCode.Internal, "Internal Server Error: " + ex.Message));
        }
    }
}