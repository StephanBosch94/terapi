using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ULearn.Be.Services.Utils;

public static class PasswordUtil
{
    public static string HashPassword(string password)
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