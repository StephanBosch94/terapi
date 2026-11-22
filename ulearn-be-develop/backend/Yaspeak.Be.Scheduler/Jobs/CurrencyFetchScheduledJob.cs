using Quartz;
using ULearn.Be.Shared.Services.Interfaces;

namespace Yaspeak.Be.Scheduler.Jobs;

public class CurrencyFetchScheduledJob : IJob
{
    public static readonly JobKey Key = new("c-currency-fetch-job");
    private readonly ICCurrencySharedService _cCurrencySharedService;

    public CurrencyFetchScheduledJob(ICCurrencySharedService cCurrencySharedService)
    {
        _cCurrencySharedService = cCurrencySharedService;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return _cCurrencySharedService.UpdateCurrencies();
    }
}