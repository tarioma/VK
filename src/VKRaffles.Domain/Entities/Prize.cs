using Ardalis.GuardClauses;
using GuardClauses;

namespace VKRaffles.Domain.Entities;

public class Prize
{
    internal const int MaxNameLength = 100;
    internal const int MinCount = 1;
    internal const int MaxCount = 100;

    public Prize(Guid id, string name, int count, Guid raffleId)
    {
        Guard.Against.Default(id);
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.StringTooLong(name, MaxNameLength);
        Guard.Against.OutOfRange(count, nameof(count), MinCount, MaxCount);
        Guard.Against.Default(raffleId);

        Id = id;
        Name = name.Trim();
        Count = count;
        RaffleId = raffleId;
    }

    public Guid Id { get; }
    public string Name { get; }
    public int Count { get; }
    public Guid RaffleId { get; }

    public static Prize Create(string name, int count, Guid raffleId)
    {
        var id = Guid.NewGuid();

        return new Prize(id, name, count, raffleId);
    }
}