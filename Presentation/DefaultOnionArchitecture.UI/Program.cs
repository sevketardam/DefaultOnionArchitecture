using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using DefaultOnionArchitecture.Application;
using DefaultOnionArchitecture.Application.Exceptions;
using DefaultOnionArchitecture.Infrastructure;
using DefaultOnionArchitecture.Infrastructure.Extensions;
using DefaultOnionArchitecture.Mapper;
using DefaultOnionArchitecture.Persistence;
using DefaultOnionArchitecture.Persistence.Data;
using DefaultOnionArchitecture.UI.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CanonicalUrlFilter>();
});

var env = builder.Environment;


builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Configuration
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);


builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = int.Parse(builder.Configuration["Kestrel:Limits:MaxRequestBodySize"] ?? "0");
    serverOptions.Limits.MaxConcurrentConnections = int.Parse(builder.Configuration["Kestrel:Limits:MaxConcurrentConnections"] ?? "0");
});

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddCustomMapper();


builder.Services.AddSession();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<EmptyResultFilter>();
});

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var method = httpContext.Request.Method.ToUpperInvariant();
        var key = $"{method}:{httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString()}";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: key,
            factory: _ =>
            {
                return method switch
                {
                    "GET" => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 50,
                        Window = TimeSpan.FromMinutes(1)
                    },
                    "POST" => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1)
                    },
                    _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1)
                    }
                };
            });
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync(
            "Çok fazla istekte bulundunuz. Lütfen sonra tekrar deneyin.", cancellationToken: token);
    };
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ConfigureExceptionHandlingMiddleware();
    //app.UseExceptionLogging();
}
else
{
    app.UseExceptionLogging();
}

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 404)
    {
        response.Redirect("/not-found");
    }
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.Services.AddLang();

app.Services.AddRoles();

app.UseRateLimiter();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
