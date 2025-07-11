using System.Text.Json;
using DefaultOnionArchitecture.Application.DTOs;
using DefaultOnionArchitecture.Application.Interface.SEO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DefaultOnionArchitecture.Infrastructure.Services;

public class SitemapService : ISitemapService
{
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _filePath;

    public SitemapService(IWebHostEnvironment env,IHttpContextAccessor httpContextAccessor)
    {
        _env = env;
        _httpContextAccessor = httpContextAccessor;
        _filePath = Path.Combine(_env.WebRootPath, "sitemap.json");
    }

    public async Task UpdateAsync(string content)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        await File.WriteAllTextAsync(_filePath, content);
    }

    public async Task<string> GetSitemapAsync()
    {
        var filePath = Path.Combine(_env.WebRootPath, "sitemap.json");

        if (!File.Exists(filePath))
            await File.WriteAllTextAsync(filePath, "[]");

        var jsonContent = await File.ReadAllTextAsync(filePath);
        return jsonContent;
    }

    public async Task<List<SitemapItemDto>> GetSitemapItemsAsync()
    {
        var filePath = Path.Combine(_env.WebRootPath, "sitemap.json");

        if (!File.Exists(filePath))
            return new List<SitemapItemDto>();

        var jsonContent = await File.ReadAllTextAsync(filePath);
        var items = JsonSerializer.Deserialize<List<SitemapItemDto>>(jsonContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "https://localhost";

        foreach (var item in items!)
        {
            item.Loc = baseUrl.TrimEnd('/') + item.Loc;
        }

        return items!;
    }
}
