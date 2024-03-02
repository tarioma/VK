using Ardalis.GuardClauses;

namespace VKRaffles.Domain.Entities;

public record Winner
{
    public Winner(long participantVkId, Guid prizeId)
    {
        Guard.Against.Default(participantVkId);
        Guard.Against.Default(prizeId);

        ParticipantVkId = participantVkId;
        PrizeId = prizeId;
    }

    public long ParticipantVkId { get; }
    public Guid PrizeId { get; }
}