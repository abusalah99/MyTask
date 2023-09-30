namespace MyTask;

public class FileService : IFileService
{
    public async Task DeleteFileIfExists(string filePath)
    {
        if (File.Exists(filePath))
            await Task.Run(() => File.Delete(filePath));
        
    }

    public async Task Save(IFormFile file, string filePath)
    {
        Stream fileStream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(fileStream);
        fileStream.Close();
    }
    public async Task<byte[]> Get(string filePath) => await File.ReadAllBytesAsync(filePath);   

}
