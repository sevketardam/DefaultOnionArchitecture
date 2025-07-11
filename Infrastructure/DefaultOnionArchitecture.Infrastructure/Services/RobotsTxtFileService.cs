using DefaultOnionArchitecture.Application.Interface.SEO;
using Microsoft.AspNetCore.Hosting;

namespace DefaultOnionArchitecture.Infrastructure.Services;

public class RobotsTxtFileService : IRobotsTxtFileService
{
    private readonly IWebHostEnvironment _env;
    private readonly string _filePath;

    public RobotsTxtFileService(IWebHostEnvironment env)
    {
        _env = env;
        _filePath = Path.Combine(_env.WebRootPath, "robots.txt");
    }

    public async Task<string> GetAsync()
    {
        if (!File.Exists(_filePath))
            await CreateDefaultAsync();

        return await File.ReadAllTextAsync(_filePath);
    }

    public async Task UpdateAsync(string content)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        await File.WriteAllTextAsync(_filePath, content);
    }

    private async Task CreateDefaultAsync()
    {
        var defaultContent = "User-agent: *\nDisallow:";
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        await File.WriteAllTextAsync(_filePath, defaultContent);
    }
}
