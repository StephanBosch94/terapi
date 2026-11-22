using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Repositories.Interfaces;

public interface ICClassDurationRepository
{
    Task<CClassDurationVM> Get(int classDurationId);
}