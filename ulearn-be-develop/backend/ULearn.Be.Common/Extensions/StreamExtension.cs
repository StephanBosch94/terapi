using Newtonsoft.Json;

namespace ULearn.Be.Common.Extensions;

public static class StreamExtension
{
    public static Stream Rewind(this Stream stream)
    {
        stream.Position = 0L;
        return stream;
    }

    public static MemoryStream GetMemoryStreamFromJson(object value)
    {
        var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, leaveOpen: true))
        using (var jsonWriter = new JsonTextWriter(writer))
        {
            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, value);
            jsonWriter.Flush();
        }

        return stream;
    }

    public static byte[] ReadAllBytes(this Stream instream)
    {
        if (instream is MemoryStream stream)
        {
            return stream.ToArray();
        }

        using var memoryStream = new MemoryStream();
        instream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}