using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CWeeklyGoalEnum
{
    [EnumKey("one_lesson_per_week")] OneLessonPerWeek = 1,
    [EnumKey("two_lessons_per_week")] TwoLessonsPerWeek = 2,
    [EnumKey("three_lessons_per_week")] ThreeLessonsPerWeek = 3,
    [EnumKey("four_lessons_per_week")] FourLessonsPerWeek = 4,

    [EnumKey("five_and_more_lessons_per_week")]
    FiveAndMoreLessonsPerWeek = 5
}