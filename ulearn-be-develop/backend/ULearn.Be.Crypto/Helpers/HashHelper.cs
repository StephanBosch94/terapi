using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Newtonsoft.Json;
using ULearn.Be.Crypto.Options;

namespace ULearn.Be.Crypto.Helpers;

public static class HashHelper
{
    public static void Hash(HashOptions options)
    {
        var hashedPasswords = GetPasswords(options)
            .Select(password => password.Split(":"))
            .Select(
                parts => new UserPassword
                {
                    Username = parts.Length != 2 ? "unknown" : parts[0],
                    Password = HashPassword(parts[parts.Length != 2 ? 0 : 1])
                }
            )
            .ToList();

        Console.Out.WriteLine(JsonConvert.SerializeObject(hashedPasswords));
    }

    private static IEnumerable<string> GetPasswords(HashOptions options)
    {
        if (options.FileName == null)
        {
            return options.Passwords;
        }

        try
        {
            return System.IO.File.ReadAllLines(options.FileName)
                .Where(o => o.Trim().Length > 0);
        }
        catch (Exception e)
        {
            throw new Exception($"Cannot read file [{options.FileName}]: {e.Message}");
        }
    }

    private static string HashPassword(string password)
    {
        return Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password,
                Encoding.ASCII.GetBytes(string.Empty),
                KeyDerivationPrf.HMACSHA1,
                10000,
                32
            )
        );
    }
}

public class UserPassword
{
    [JsonProperty(PropertyName = "username")]
    public string Username { get; set; }

    [JsonProperty(PropertyName = "password")]
    public string Password { get; set; }
}