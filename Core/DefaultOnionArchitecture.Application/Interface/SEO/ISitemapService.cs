using DefaultOnionArchitecture.Application.DTOs;

namespace DefaultOnionArchitecture.Application.Interface.SEO;

public interface ISitemapService
{
    Task<List<SitemapItemDto>> GetSitemapItemsAsync();
    Task<string> GetSitemapAsync();
    Task UpdateAsync(string content);
}
