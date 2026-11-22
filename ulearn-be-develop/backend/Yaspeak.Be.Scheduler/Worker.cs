using Quartz;
using Yaspeak.Be.Scheduler.Jobs;

namespace Yaspeak.Be.Scheduler;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // Create a Quartz.NET scheduler
        var schedulerFactory = _serviceProvider.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
        await scheduler.Start(cancellationToken);

        await ScheduleAppTransactionJob(scheduler, cancellationToken);
        await ScheduleNotificationJob(scheduler, cancellationToken);
        await ScheduleCouponGenerationJob(scheduler, cancellationToken);
        await ScheduleCurrencyFetchJob(scheduler, cancellationToken);
    }

    private async Task ScheduleAppTransactionJob(IScheduler scheduler, CancellationToken cancellationToken)
    {
        const int intervalInMinutes = 1;
        var job = JobBuilder.Create<AppTransactionScheduledJob>()
            .WithIdentity(AppTransactionScheduledJob.Key)
            .Build();
        var trigger = TriggerBuilder.Create()
            .WithIdentity(AppTransactionScheduledJob.Key.Name + "-trigger")
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInMinutes(intervalInMinutes).RepeatForever())
            .Build();
        await scheduler.ScheduleJob(job, trigger, cancellationToken);

        _logger.LogDebug(
            "App transaction job has started with interval in minutes: {@ScheduleCronExpression}",
            intervalInMinutes
        );
    }

    private async Task ScheduleNotificationJob(IScheduler scheduler, CancellationToken cancellationToken)
    {
        const int intervalInMinutes = 1;
        var job = JobBuilder.Create<NotificationScheduledJob>()
            .WithIdentity(NotificationScheduledJob.Key)
            .Build();
        var trigger = TriggerBuilder.Create()
            .WithIdentity(NotificationScheduledJob.Key.Name + "-trigger")
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInMinutes(intervalInMinutes).RepeatForever())
            .Build();
        await scheduler.ScheduleJob(job, trigger, cancellationToken);

        _logger.LogDebug(
            "App notification job has started with interval in minutes: {@ScheduleCronExpression}",
            intervalInMinutes
        );
    }

    private async Task ScheduleCouponGenerationJob(IScheduler scheduler, CancellationToken cancellationToken)
    {
        const int intervalInMinutes = 5;
        var job = JobBuilder.Create<AppCouponScheduledJob>()
            .WithIdentity(AppCouponScheduledJob.Key)
            .Build();
        var trigger = TriggerBuilder.Create()
            .WithIdentity(AppCouponScheduledJob.Key.Name + "-trigger")
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInMinutes(intervalInMinutes).RepeatForever())
            .Build();
        await scheduler.ScheduleJob(job, trigger, cancellationToken);

        _logger.LogDebug(
            "Schedule coupon generation job has started with interval in minutes: {@ScheduleCronExpression}",
            intervalInMinutes
        );
    }

    private async Task ScheduleCurrencyFetchJob(IScheduler scheduler, CancellationToken cancellationToken)
    {
        const int intervalInHours = 3;
        var job = JobBuilder.Create<CurrencyFetchScheduledJob>()
            .WithIdentity(CurrencyFetchScheduledJob.Key)
            .Build();
        var trigger = TriggerBuilder.Create()
            .WithIdentity(CurrencyFetchScheduledJob.Key.Name + "-trigger")
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInHours(intervalInHours).RepeatForever())
            .Build();
        await scheduler.ScheduleJob(job, trigger, cancellationToken);

        _logger.LogDebug(
            "Schedule currency fetch job has started with interval in hours: {@ScheduleCronExpression}",
            intervalInHours
        );
    }
}