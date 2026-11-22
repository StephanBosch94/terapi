using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppUserRegistration;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppUserRegistrationCodeRepository
{
    Task Add(int userId, string code);
    Task<AppUserRegistrationCode> GetLatestDbEntity(int userId, bool asNoTracking = true);
    Task SetUsed(int userRegistrationCodeId);
    Task<UserRegistrationCodeInfoVM> GetLatestCodeInfo(int userId);
    Task<AppUserRegistrationCode> GetRawDbEntity(int userId, string registrationCode);
}