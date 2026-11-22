using System.Security.Cryptography;
using ULearn.Be.Common.Services.Interfaces;

namespace ULearn.Be.Common.Services.Utilities;

public class GuidUtility : IGuidUtility
{
    private readonly RandomNumberGenerator _randomGenerator = RandomNumberGenerator.Create();

    public string GenerateGuidString()
    {
        return GenerateGuid().ToString();
    }

    public Guid GenerateGuid()
    {
        var randomBytes = new byte[16];
        _randomGenerator.GetBytes(randomBytes);

        return new Guid(randomBytes);
    }

    public string GenerateCode() => Guid.NewGuid().ToString("N");
    public string GenerateShortCode() => string.Join("", GenerateCode().Take(6)).ToUpper();
}