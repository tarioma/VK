using Ardalis.GuardClauses;

namespace VKRaffles.Domain.Entities;

public class Participant
{
    public Participant(Guid id, long vkId, Guid raffleId)
    {
        Guard.Against.Default(id);
        Guard.Against.NegativeOrZero(vkId);
        Guard.Against.Default(raffleId);

        Id = id;
        VkId = vkId;
        RaffleId = raffleId;
    }

    public Guid Id { get; }
    public long VkId { get; }
    public Guid RaffleId { get; }

    public static Participant Create(long vkId, Guid raffleId)
    {
        var id = Guid.NewGuid();

        return new Participant(id, vkId, raffleId);
    }
}