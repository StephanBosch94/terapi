using ULearn.Be.Data.Enums;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Services.Services.Interfaces;

namespace ULearn.Be.Services.Services;

public class MasterHashService: IMasterHashService
{
    private readonly IMasterHashRepository _masterHashRepository;

    public MasterHashService(IMasterHashRepository masterHashRepository)
    {
        _masterHashRepository = masterHashRepository;
    }

    public Task<DateTime> GetLastUpdated(CMasterHashEnum masterHash)
    {
        return _masterHashRepository.GetLastUpdate(masterHash);
    }
}