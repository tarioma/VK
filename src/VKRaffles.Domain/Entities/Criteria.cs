namespace VKRaffles.Domain.Entities;

[Flags]
public enum Criteria
{
    None = 0,
    Like = 1,
    Repost = 2,
    Comment = 4,
    Subscription = 8,
    SecondGroupSubscription = 16
}