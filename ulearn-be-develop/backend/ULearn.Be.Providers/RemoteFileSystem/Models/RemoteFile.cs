namespace ULearn.Be.Providers.RemoteFileSystem.Models;

public class RemoteFile
{
    public Stream Stream { get; set; }

    public string Name { get; set; }

    public string ContentType { get; set; }

    public string Path { get; set; }
}
