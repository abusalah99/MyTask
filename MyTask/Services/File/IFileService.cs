namespace MyTask;

public interface IFileService 
{
    Task Save(IFormFile file, string FilePath);

    Task DeleteFileIfExists(string filePath);

    Task<byte[]> Get(string filePath);
}
