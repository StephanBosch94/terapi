using ImageMagick;
using Microsoft.AspNetCore.Http;

namespace ULearn.Be.Services.Utils;

public static class ImageCompressionUtil
{
    public static byte[] Compress(IFormFile image)
    {
        byte[] result = null;

        // filestream
        using var fileStream = image.OpenReadStream();
        using var memoryStream = new MemoryStream();
        fileStream.CopyTo(memoryStream);
        memoryStream.Position = 0; // The position needs to be reset.

        var optimizer = new ImageOptimizer();
        optimizer.LosslessCompress(memoryStream);

        // convert to byte[]
        result = memoryStream.ToArray();

        return result;
    }
}