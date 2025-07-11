using Microsoft.AspNetCore.Builder;

namespace DefaultOnionArchitecture.Infrastructure.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionLoggingMiddleware>();
    }
}
