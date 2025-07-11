using Microsoft.AspNetCore.Http;

namespace DefaultOnionArchitecture.Application.Interface.ExceptionLogger;

public interface IExceptionLogger
{
    Task<string> LogAsync(HttpContext context, Exception exception);
}
