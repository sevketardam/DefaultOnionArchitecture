namespace DefaultOnionArchitecture.Application.Interface.SEO;

public interface IRobotsTxtFileService
{
    Task<string> GetAsync();
    Task UpdateAsync(string content);
}
