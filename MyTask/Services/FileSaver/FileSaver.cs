namespace MyTask;

public class FileSaver : IFileSaver
{
    public async Task DeleteFileIfExists(string filePath)
    {
        if (File.Exists(filePath))
        {
            try
            {
                await Task.Run(() => File.Delete(filePath));
            }
            catch(Exception ex) 
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }

    public async Task Save(IFormFile file, string filePath)
    {
        Stream fileStream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(fileStream);
        fileStream.Close();
    }

}
