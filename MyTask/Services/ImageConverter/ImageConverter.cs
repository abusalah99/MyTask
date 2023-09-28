namespace MyTask;

public class ImageConverter : IImageConverter
{
    public async Task<byte[]> ConvertImage(IFormFile image)
    {
        MemoryStream byteImage = new ();
        await image.CopyToAsync (byteImage);
        byte [] imageArray = byteImage.ToArray();

        byteImage.Dispose();

        return imageArray;
    }
}
