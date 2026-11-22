namespace ULearn.Be.Models.InputModels;

public record AppUserReviewUpdateIM : AppUserReviewIM
{
    public int UserReviewId { get; set; }
}