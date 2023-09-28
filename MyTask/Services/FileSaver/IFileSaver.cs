namespace MyTask;

public interface IFileSaver 
{
    Task Save(IFormFile file, string FilePath);

    Task DeleteFileIfExists(string filePath);
}
