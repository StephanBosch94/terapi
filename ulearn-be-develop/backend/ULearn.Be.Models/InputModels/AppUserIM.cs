namespace ULearn.Be.Models.InputModels;

public class AppUserIM
{
    public string Login { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Timezone { get; set; }
    public string Ico { get; set; }
    public string PlaceOfBusiness { get; set; }
    public string Dic { get; set; }
    public int? TranslationLanguageId { get; set; }
    public int? CountryId { get; set; }
    public int? TeachingExperienceId { get; set; }
    public int? CurrencyId { get; set; }
    public double? HourRate { get; set; }
    public int? SubjectId { get; set; }
    public int UserRoleId { get; set; }
    public int UserStateId { get; set; }
    public int AuthenticationMethodId { get; set; }
    public int? WeeklyGoalId { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankCode { get; set; }
    public string CompanyName { get; set; }
    public string BillingEmail { get; set; }
    public bool IsVatPayer { get; set; }
    public ICollection<AppUserAttributeIM> Attributes { get; set; }
    public ICollection<AppUserLanguageIM> Languages { get; set; }
    public ICollection<int> SpecializationIds { get; set; }
    public ICollection<int> SubjectLevelIds { get; set; }
    public ICollection<int> InterestIds { get; set; }
}