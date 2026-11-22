namespace ULearn.Be.Models.InputModels;

public record AppUserReviewIM
{
    public int UserId { get; set; }
    public int Value { get; set; }
    public string TextValue { get; set; }
}