using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Common.Services.Interfaces;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.InputModels;
using ULearn.Be.Models.ViewModels.AppUser;
using ULearn.Be.Models.ViewModels.Auth;
using ULearn.Be.Providers.Auth.Interfaces;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;
using ULearn.Be.Services.Utils;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class AuthService : IAuthService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly ICTranslationLanguageRepository _cTranslationLanguageRepository;
    private readonly IFacebookProvider _facebookProvider;
    private readonly IGoogleApiProvider _googleApiProvider;
    private readonly IGuidUtility _guidUtility;
    private readonly IHubspotSharedService _hubspotSharedService;
    private readonly IInternalAuthProvider _internalAuthProvider;
    private readonly ILogger<AuthService> _logger;
    private readonly INotificationSharedService _notificationSharedService;
    private readonly IRUserTokenRepository _rUserTokenRepository;
    private readonly ISeznamApiProvider _seznamApiProvider;
    private readonly ITokenService _tokenService;
    private readonly IRUserFileRepository _userFileRepository;

    public AuthService(
        IGoogleApiProvider googleApiProvider,
        IAppUserRepository appUserRepository,
        IRUserTokenRepository rUserTokenRepository,
        IInternalAuthProvider internalAuthProvider,
        ISeznamApiProvider seznamApiProvider,
        IFacebookProvider facebookProvider,
        IRUserFileRepository userFileRepository,
        IGuidUtility guidUtility,
        ILogger<AuthService> logger,
        ITokenService tokenService,
        ICTranslationLanguageRepository cTranslationLanguageRepository,
        IHubspotSharedService hubspotSharedService,
        INotificationSharedService notificationSharedService
    )
    {
        _googleApiProvider = googleApiProvider;
        _appUserRepository = appUserRepository;
        _rUserTokenRepository = rUserTokenRepository;
        _internalAuthProvider = internalAuthProvider;
        _seznamApiProvider = seznamApiProvider;
        _facebookProvider = facebookProvider;
        _userFileRepository = userFileRepository;
        _guidUtility = guidUtility;
        _logger = logger;
        _tokenService = tokenService;
        _cTranslationLanguageRepository = cTranslationLanguageRepository;
        _hubspotSharedService = hubspotSharedService;
        _notificationSharedService = notificationSharedService;
    }

    public async Task<ICollection<Claim>> LoginGoogle(AuthExternalSignUpVM input)
    {
        var externalAuthTokenVm = await _googleApiProvider.Authenticate(input);
        var appUserVm = await UpsertUser(
            externalAuthTokenVm,
            input.UserRoleId,
            CUserStateEnum.RoleSetup,
            CAuthenticationMethodEnum.Google,
            input.TimeZone,
            input.LanguageCode
        );
        await UpsertTokens(appUserVm.UserId, externalAuthTokenVm.Tokens);

        return AuthUtil.GetClaims(appUserVm);
    }

    public async Task<ICollection<Claim>> LoginGoogleOneTap(GoogleOneTapLoginIM input)
    {
        var externalAuthTokenVm = await _googleApiProvider.ValidateToken(input.Token);
        var appUserVm = await UpsertUser(
            externalAuthTokenVm,
            input.UserRoleId,
            CUserStateEnum.RoleSetup,
            CAuthenticationMethodEnum.GoogleOneTap,
            input.TimeZone,
            input.LanguageCode
        );
        return AuthUtil.GetClaims(appUserVm);
    }

    public async Task<ICollection<Claim>> LoginFacebook(AuthExternalSignUpVM input)
    {
        var externalAuthTokenVm = await _facebookProvider.Authenticate(input);

        var appUserVm = await UpsertUser(
            externalAuthTokenVm,
            input.UserRoleId,
            CUserStateEnum.RoleSetup,
            CAuthenticationMethodEnum.Facebook,
            input.TimeZone,
            input.LanguageCode
        );
        await UpsertTokens(appUserVm.UserId, externalAuthTokenVm.Tokens);

        return AuthUtil.GetClaims(appUserVm);
    }

    public async Task<ICollection<Claim>> LoginSeznam(AuthExternalSignUpVM input)
    {
        var externalAuthTokenVm = await _seznamApiProvider.Authenticate(input);

        var appUserVm = await UpsertUser(
            externalAuthTokenVm,
            input.UserRoleId,
            CUserStateEnum.RoleSetup,
            CAuthenticationMethodEnum.Seznam,
            input.TimeZone,
            input.LanguageCode
        );
        await UpsertTokens(appUserVm.UserId, externalAuthTokenVm.Tokens);

        return AuthUtil.GetClaims(appUserVm);
    }

    public async Task<ICollection<Claim>> LoginInternal(AuthInternalLoginIM input)
    {
        await _internalAuthProvider.Authenticate(input);
        var appUserVm = await _appUserRepository.GetByLogin(input.Login, onlyConfirmed: false);
        return AuthUtil.GetClaims(appUserVm);
    }

    public async Task<AuthTokenVM> LoginInternalBearer(AuthInternalLoginIM authInternalLoginIm)
    {
        await _internalAuthProvider.Authenticate(authInternalLoginIm);
        var appUserVm = await _appUserRepository.GetByLogin(authInternalLoginIm.Login, onlyConfirmed: false);
        return GetAuthTokenVm(appUserVm);
    }

    private AuthTokenVM GetAuthTokenVm(AppUserBasicVM appUserVm)
    {
        var token = _tokenService.GetToken(AuthUtil.GetClaims(appUserVm));
        return new AuthTokenVM
        {
            UserId = appUserVm.UserId,
            Login = appUserVm.Login,
            Email = appUserVm.Email,
            Name = appUserVm.Name,
            FirstName = appUserVm.FirstName,
            LastName = appUserVm.LastName,
            Created = appUserVm.Created,
            Phone = appUserVm.Phone,
            UserRoleId = appUserVm.UserRoleId,
            StateId = appUserVm.StateId,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }

    private async Task UpsertTokens(int userId, IEnumerable<ExternalUserTokenVM> externalUserTokenVms)
    {
        var tokenIMs = externalUserTokenVms.Select(
            x => new UserTokenIM
            {
                CTokenProviderEnum = x.CTokenProviderEnum,
                CTokenTypeEnum = x.CTokenTypeEnum,
                TokenValue = x.TokenValue
            }
        ).ToList();
        if (!tokenIMs.Any())
        {
            return;
        }

        await _rUserTokenRepository.Upsert(userId, tokenIMs);
    }

    private async Task<AppUserBasicVM> UpsertUser(
        AuthUserFromExternalSourceVM authExternalVm,
        CUserRoleEnum cUserRoleEnum,
        CUserStateEnum cUserStateEnum,
        CAuthenticationMethodEnum cAuthenticationMethodEnum,
        string timezone,
        string languageCode
    )
    {
        var userIm = new AppUserIM
        {
            Login = authExternalVm.Email,
            Email = authExternalVm.Email,
            Name = authExternalVm.Name,
            FirstName = authExternalVm.FirstName,
            LastName = authExternalVm.LastName,
            Phone = authExternalVm.Phone,
            Timezone = timezone,
            UserStateId = (int)cUserStateEnum,
            UserRoleId = (int)cUserRoleEnum,
            AuthenticationMethodId = (int)cAuthenticationMethodEnum,
            CurrencyId = (int)CCurrencyEnum.Czk,
            TranslationLanguageId = await _cTranslationLanguageRepository.GetTranslationLanguageId(languageCode)
        };
        var (user, created) = await _appUserRepository.AddIfNotExists(userIm, authExternalVm.Email);
        await UpsertImages(user.UserId, authExternalVm.PictureUrl);
        //await _hubspotSharedService.SyncUser(user.UserId);
        if (created)
        {
            await _notificationSharedService.SendAccountCreated(user.UserId);
        }

        return await _appUserRepository.GetByLogin(authExternalVm.Email, false);
    }

    private async Task UpsertImages(int userId, string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }

        try
        {
            var exists = await _userFileRepository.Exists(userId, CFileTypeEnum.ProfilePhoto);
            if (exists)
            {
                return;
            }

            var (memeType, file) = await DownloadImage(url);
            var content = file.ToArray();
            var fileName = url.Split("/").Last();
            var profilePicture = new UserFileInputVM
            {
                FileTypeId = CFileTypeEnum.ProfilePhoto,
                Directory = $"{CFileTypeEnum.ProfilePhoto.GetEnumKey()}/{_guidUtility.GenerateGuidString()}",
                ContentType = memeType,
                FileName = Path.GetFileName(fileName),
            };
            var profileThumbnail = new UserFileInputVM
            {
                FileTypeId = CFileTypeEnum.ProfilePhotoThumbnail,
                Directory = $"{CFileTypeEnum.ProfilePhotoThumbnail.GetEnumKey()}/{_guidUtility.GenerateGuidString()}",
                ContentType = memeType,
                FileName = Path.GetFileName(fileName),
            };

            await _userFileRepository.AddWithContent(profilePicture, content, userId);
            await _userFileRepository.AddWithContent(profileThumbnail, content, userId);
        } catch (Exception ex)
        {
            _logger.LogError("DB file storage error {err}", ex.Message);
        }
    }

    private async Task<(string memeType, MemoryStream stream)> DownloadImage(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return (null, null);
        }

        try
        {
            using var client = new HttpClient();

            var response = await client.GetAsync(GetUrlWithBetterImageQuality(url));
            response.EnsureSuccessStatusCode();

            var contentType = response.Content.Headers.ContentType?.MediaType;
            await using var inputStream = await response.Content.ReadAsStreamAsync();
            var outputStream = new MemoryStream();

            await inputStream.CopyToAsync(outputStream);
            _logger.LogDebug("Image downloaded successfully!");
            return (contentType, outputStream);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error downloading image: {ExMessage}", ex.Message);
        }

        return (null, null);
    }

    private string GetUrlWithBetterImageQuality(string url)
    {
        const string defaultParam = "=s96-c";
        const string targetParam = "=s2048-c";
        return !string.IsNullOrEmpty(url)
            ? url.Replace(defaultParam, targetParam)
            : url;
    }
}