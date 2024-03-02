using Ardalis.GuardClauses;
using GuardClauses;

namespace VKRaffles.Domain.Entities;

public record Prize
{
    internal const int MaxNameLength = 100;
    internal const int MaxCount = 100;

    public Prize(Guid id, string name, int count, Guid raffleId)
    {
        Guard.Against.Default(id);
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.StringTooLong(name, MaxNameLength);
        Guard.Against.OutOfRange(count, nameof(count), 1, MaxCount);
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
}