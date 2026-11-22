namespace ULearn.Be.File.Service.Services;

public interface ICompressionService
{
    byte[] CompressZip(byte[] data);
    byte[] DecompressZip(byte[] compressed);
}
