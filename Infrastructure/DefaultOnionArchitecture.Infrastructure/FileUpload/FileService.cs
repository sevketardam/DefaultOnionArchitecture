using DefaultOnionArchitecture.Application.Interface.FileUpload;
using Microsoft.AspNetCore.Http;

namespace DefaultOnionArchitecture.Infrastructure.FileUpload;

public class FileService : IFileService
{
    private readonly string _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

    public async Task<string> UploadAsync(IFormFile file, string folder)
    {
        string folderPath = Path.Combine(_rootPath, folder);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
        string fileExtension = Path.GetExtension(file.FileName);

        fileNameWithoutExtension = ConvertTurkishCharacters(fileNameWithoutExtension);

        fileNameWithoutExtension = fileNameWithoutExtension.ToLower();
        fileNameWithoutExtension = System.Text.RegularExpressions.Regex.Replace(fileNameWithoutExtension, @"[^a-z0-9]", "-");
        fileNameWithoutExtension = fileNameWithoutExtension.Replace(" ", "-"); 

        string filePath = Path.Combine(folderPath, fileNameWithoutExtension + fileExtension);

        int counter = 1;
        string newFilePath = filePath;

        while (File.Exists(newFilePath))
        {
            string newFileName = $"{fileNameWithoutExtension}-{counter}{fileExtension}";
            newFilePath = Path.Combine(folderPath, newFileName);
            counter++;
        }

        using (var stream = new FileStream(newFilePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine("uploads", folder, Path.GetFileName(newFilePath));
    }

    private string ConvertTurkishCharacters(string input)
    {
        var turkishChars = new Dictionary<char, char>
        {
            { 'ö', 'o' },
            { 'Ö', 'o' },  
            { 'ç', 'c' },
            { 'Ç', 'c' }, 
            { 'ı', 'i' },
            { 'I', 'i' },
            { 'ş', 's' },
            { 'Ş', 's' },
            { 'ğ', 'g' },
            { 'Ğ', 'g' },
            { 'ü', 'u' },
            { 'Ü', 'u' }
        };

        foreach (var character in turkishChars)
        {
            input = input.Replace(character.Key, character.Value);
        }

        return input;
    }

    public async Task<byte[]> DownloadAsync(string fileName)
    {
        var filePath = Path.Combine(_rootPath, fileName);
        return await File.ReadAllBytesAsync(filePath);
    }

    public bool Delete(string fileName)
    {
        var filePath = Path.Combine(_rootPath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }
        return false;
    }

    public bool DeleteRange(List<string> fileNameList)
    {
        var status = false;
        foreach (var fileName in fileNameList)
        {
            var filePath = Path.Combine(_rootPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                status = true;
                continue;
            }
            status = false;
            break;
        }

        return status;

    }
}
