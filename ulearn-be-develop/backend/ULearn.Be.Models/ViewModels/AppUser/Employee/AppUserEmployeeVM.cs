using ULearn.Be.Models.Filter;
using ULearn.Be.Models.ViewModels.Codetables;
using ULearn.Be.Models.ViewModels.File;

namespace ULearn.Be.Models.ViewModels.AppUser.Employee;

public record AppUserEmployeeVM : AppUserEmployeeFilterVM
{
    public int UserId { get; set; }
    public int UserRoleId { get; set; }
    public new int UserStateId { get; set; }
    public string Email { get; set; }
    public string Timezone { get; set; }
    public string Ico { get; set; }
    public string Dic { get; set; }
    public int? TranslationLanguageId { get; set; }
    public int? WeeklyGoalId { get; set; }
    public int CurrentWeeklyGoalProgress { get; set; }
    public CUserStateVM State { get; set; }
    public AppUserEmployeeWalletVM Wallet { get; set; }
    public IEnumerable<UserFileVM> Files { get; set; }
    public AppUserEmployeeLessonStatisticsVM LessonStatistics { get; set; }
}