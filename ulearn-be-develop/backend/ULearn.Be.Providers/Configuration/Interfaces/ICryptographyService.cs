namespace ULearn.Be.Providers.Configuration.Interfaces;

public interface ICryptographyService
{
    string Encrypt(string value, string password);

    string Decrypt(string value, string password);
}