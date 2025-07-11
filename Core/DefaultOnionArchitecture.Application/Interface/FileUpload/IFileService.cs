using Microsoft.AspNetCore.Http;

namespace DefaultOnionArchitecture.Application.Interface.FileUpload;

public interface IFileService
{
    Task<string> UploadAsync(IFormFile file, string folder);
    Task<byte[]> DownloadAsync(string fileName);
    bool Delete(string fileName);
    bool DeleteRange(List<string> fileName);
}
