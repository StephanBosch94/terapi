using System.Security.Cryptography;
using System.Text;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Providers.Configuration.Interfaces;

namespace ULearn.Be.Providers.Configuration;

public class CryptographyService : ICryptographyService
{
    private const string Salt = "31qk260ys571b31s";
    private const string Vector = "mwr523n228nqjys6";
    private const int Iterations = 100;

    private const int KeySize = 256;

    /* Hash Algorithm must be set to SHA1 as Pwd.Merge uses .Net Framework 4.6 */
    /* and it does not allow change Hash Algorithm  */
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA1;

    public string Encrypt(string value, string password)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var vectorBytes = Encoding.ASCII.GetBytes(Vector);
        var saltBytes = Encoding.ASCII.GetBytes(Salt);
        var valueBytes = Encoding.UTF8.GetBytes(value);

        byte[] encrypted;
        using var cipher = Aes.Create();
        var passwordBytes =
            new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithm);
        var keyBytes = passwordBytes.GetBytes(KeySize / 8);

        cipher.Mode = CipherMode.CBC;

        try
        {
            using var encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes);
            using var to = new MemoryStream();
            using var writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write);
            writer.Write(valueBytes, 0, valueBytes.Length);
            writer.FlushFinalBlock();
            encrypted = to.ToArray();
        }
        finally
        {
            cipher.Clear();
        }

        return Convert.ToBase64String(encrypted);
    }

    public string Decrypt(string value, string password)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var vectorBytes = Encoding.ASCII.GetBytes(Vector);
        var saltBytes = Encoding.ASCII.GetBytes(Salt);
        var valueBytes = Convert.FromBase64String(value);

        byte[] decrypted;

        using var cipher = Aes.Create();
        var passwordBytes =
            new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithm);
        var keyBytes = passwordBytes.GetBytes(KeySize / 8);

        cipher.Mode = CipherMode.CBC;

        try
        {
            using var decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes);
            using var from = new MemoryStream(valueBytes);
            using var reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read);
            decrypted = reader.ReadAllBytes();
        }
        finally
        {
            cipher.Clear();
        }

        return Encoding.UTF8.GetString(decrypted);
    }
}