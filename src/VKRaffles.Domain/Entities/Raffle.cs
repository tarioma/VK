using Ardalis.GuardClauses;

namespace VKRaffles.Domain.Entities;

public record Raffle
{
    private const string PostIdPattern = @"^-\d{1,10}_\d{1,10}$";
    
    private readonly HashSet<Winner> _winner;
    
    public Raffle(Guid id, string postId, Criteria criteria, DateTime dateTime, Guid organizerId)
    {
        Guard.Against.Default(id);
        Guard.Against.Null(postId);
        Guard.Against.InvalidFormat(postId, nameof(postId), PostIdPattern);
        Guard.Against.Default(criteria);
        Guard.Against.Default(organizerId);

        Id = id;
        PostId = postId;
        Criteria = criteria;
        DateTime = dateTime;
        OrganizerId = organizerId;
    }
    
    public Guid Id { get; }
    public string PostId { get; }
    public Criteria Criteria { get; }
    public DateTime DateTime { get; }
    public Guid OrganizerId { get; }
    public IReadOnlyCollection<Participant> Participants { get; }
    public IReadOnlyCollection<Prize> Prizes { get; }
    public IReadOnlyCollection<Winner> Winners => _winner;

    public static Raffle Create(string postId, Criteria criteria, Guid organizerId)
    {
        var id = Guid.NewGuid();
        var dateTime = DateTime.UtcNow;

        return new Raffle(id, postId, criteria, dateTime, organizerId);
    }
}