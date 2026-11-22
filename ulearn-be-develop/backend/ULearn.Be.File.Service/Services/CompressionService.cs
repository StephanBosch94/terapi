using System.IO.Compression;

namespace ULearn.Be.File.Service.Services;

public class CompressionService : ICompressionService
{
    public byte[] CompressZip(byte[] data)
    {
        using var outputStream = new MemoryStream();
        using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
        {
            gzipStream.Write(data);
        }

        var compressed = outputStream.ToArray();
        return compressed;
    }

    public byte[] DecompressZip(byte[] compressed)
    {
        using var from = new MemoryStream(compressed);
        using var to = new MemoryStream();
        using (var gZipStream = new GZipStream(from, CompressionMode.Decompress))
        {
            gZipStream.CopyTo(to);
        }

        return to.ToArray();
    }
}
