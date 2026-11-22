using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Http; 

namespace ULearn.Be.Services.Utils;

public static class ImageConverterUtil
{
    private const int ProfilePhotoWidthAndHeight = 1024;
    private const int ProfilePhotoThumbnailWidthAndHeight = 256;

    public static Task<byte[]> ConvertAndCompressToJpeg(IFormFile file)
    {
        return ConvertAndCompressToJpeg(file, ProfilePhotoWidthAndHeight);
    }

    public static Task<byte[]> ConvertAndCompressThumbnailToJpeg(IFormFile file)
    {
        return ConvertAndCompressToJpeg(file, ProfilePhotoThumbnailWidthAndHeight);
    }

    private static async Task<byte[]> ConvertAndCompressToJpeg(IFormFile file, int widthAndHeight)
    {
        // Copy the IFormFile content to a MemoryStream
        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0; // Reset position to the beginning of the stream

        // Load the image from the MemoryStream
        using var image = await Image.LoadAsync(ms);

        // Calculate the desired dimensions for the cropped image (square based on the smaller side)
        var targetSize = Math.Min(image.Width, image.Height);

        // Process the image: crop to a square from the center, then resize
        image.Mutate(x => x
                .Crop(targetSize, targetSize) // Crop to a square (ImageSharp defaults to center)
                .Resize(widthAndHeight, widthAndHeight) // Resize to the target square dimensions
        );

        // Save the processed image to a new MemoryStream as JPEG
        await using var outputMs = new MemoryStream();
        var encoder = new JpegEncoder
        {
            Quality = 75 // You can adjust JPEG quality (0-100)
        };
        await image.SaveAsync(outputMs, encoder);

        return outputMs.ToArray();
    }
}