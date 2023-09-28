namespace MyTask;

public interface IImageConverter
{
    Task<byte[]> ConvertImage(IFormFile image); 
}
