using Ardalis.GuardClauses;

namespace VKRaffles.Domain.Entities;

public record Winner
{
    public Winner(Guid participantId, Guid prizeId)
    {
        Guard.Against.Default(participantId);
        Guard.Against.Default(prizeId);

        ParticipantId = participantId;
        PrizeId = prizeId;
    }
    
    public Guid ParticipantId { get; }
    public Guid PrizeId { get; }
}