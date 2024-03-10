using AutoFixture;

namespace VKRaffles.Domain.Tests.Tools;

public static class FixtureExtension
{
    public static string GenerateString(this IFixture fixture, int length) =>
        new(fixture.CreateMany<char>(length).ToArray());

    public static int GenerateIntFromRange(this IFixture fixture, int min, int max) =>
        fixture.Create<int>() % (max - min + 1) + min;

    public static string GenerateRafflePostId(this IFixture fixture)
    {
        var groupId = Math.Abs(fixture.Create<int>());
        var postId = Math.Abs(fixture.Create<int>());

        return $"-{groupId}_{postId}";
    }

    public static IEnumerable<long> GenerateLongRange(this IFixture _, int length) =>
        Enumerable.Range(1, length).Select(p => (long)p);
}