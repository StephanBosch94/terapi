using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Data.Context;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Providers.Auth.Interfaces;

namespace ULearn.Be.Providers.Auth;

public class InternalAuthProvider : IInternalAuthProvider
{
    private readonly RepositoryContext _db;
    private readonly ILogger<InternalAuthProvider> _logger;

    public InternalAuthProvider(
        ILogger<InternalAuthProvider> logger,
        RepositoryContext db
    )
    {
        _logger = logger;
        _db = db;
    }

    public async Task Authenticate(AuthInternalLoginIM input)
    {
        var user = await _db.AppUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Login == input.Login);
        if (user == null || user.UserStateId == (int)CUserStateEnum.Deleted)
        {
            // dont log input username, it can contain user password by mistake
            _logger.LogInformation("Failed in memory authentication for unknown user");
            throw new UnauthorizedException();
        }

        var hashedPassword = HashPassword(input.Password);
        if (!hashedPassword.Equals(user.Password, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Failed in memory authentication for {@User}", input.Login);
            throw new UnauthorizedException();
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