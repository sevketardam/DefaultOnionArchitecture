using System.Text;
using DefaultOnionArchitecture.Application.Interface.ExceptionLogger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DefaultOnionArchitecture.Infrastructure.Logger;

public class TxtExceptionLogger : IExceptionLogger
{
    private readonly IWebHostEnvironment _env;

    public TxtExceptionLogger(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> LogAsync(HttpContext context, Exception exception)
    {
        var errorCode = Guid.NewGuid().ToString();
        var utcNow = DateTime.UtcNow;
        var errorDetails = new StringBuilder();

        errorDetails.AppendLine(new string('-', 50));
        errorDetails.AppendLine($"Time (UTC): {utcNow:yyyy-MM-dd HH:mm:ss}");
        errorDetails.AppendLine($"Error Code: {errorCode}");
        errorDetails.AppendLine($"Request Path: {context.Request.Path}");
        errorDetails.AppendLine($"Message: {exception.Message}");
        errorDetails.AppendLine($"Stack Trace: {exception.StackTrace}");
        errorDetails.AppendLine(new string('-', 50));
        errorDetails.AppendLine();

        var errorsFolder = Path.Combine(_env.WebRootPath, "errors");
        if (!Directory.Exists(errorsFolder))
            Directory.CreateDirectory(errorsFolder);

        var fileName = $"{utcNow:dd-MM-yyyy}.txt";
        var filePath = Path.Combine(errorsFolder, fileName);

        await File.AppendAllTextAsync(filePath, errorDetails.ToString());

        return errorCode;
    }
}
