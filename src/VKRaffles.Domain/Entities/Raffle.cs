using Ardalis.GuardClauses;
using GuardClauses;

namespace VKRaffles.Domain.Entities;

public record Raffle
{
    internal const int MaxPrizesCount = 10;
    internal const int MaxParticipantsCount = 1_000_000;
    internal const int MaxSecondGroupSlugLength = 33;

    private const string PostIdPattern = @"^-\d{1,10}_\d{1,10}$";

    public Raffle(
        Guid id,
        string postId,
        Criteria criteria,
        DateTime dateTime,
        Guid organizerId,
        IReadOnlySet<long> participants,
        IReadOnlySet<Prize> prizes,
        string? secondGroupSlug)
    {
        Guard.Against.Default(id);
        Guard.Against.InvalidFormat(postId, nameof(postId), PostIdPattern);
        Guard.Against.Default(criteria);
        Guard.Against.Default(dateTime);
        Guard.Against.Default(organizerId);
        Guard.Against.NullOrEmpty(participants);
        Guard.Against.OutOfRange(prizes.Count, nameof(prizes), 1, MaxParticipantsCount);
        Guard.Against.NullOrEmpty(prizes);
        Guard.Against.OutOfRange(prizes.Count, nameof(prizes), 1, MaxPrizesCount);

        if (secondGroupSlug is null && (criteria & Criteria.SecondGroupSubscription) == 0)
        {
            throw new ArgumentException("Короткое имя второго сообщества передано, но критерий не выбран.");
        }

        if (secondGroupSlug is not null)
        {
            secondGroupSlug = secondGroupSlug.Trim();
            Guard.Against.StringTooLong(secondGroupSlug, MaxSecondGroupSlugLength);
        }

        if ((criteria & Criteria.Subscription) == 0 && (criteria & Criteria.SecondGroupSubscription) != 0)
        {
            throw new ArgumentException("Второе сообщество не может быть выбрано без основного.", nameof(criteria));
        }

        if (prizes.Sum(p => p.Count) > participants.Count)
        {
            throw new AggregateException("Количество призов не может быть больше количества участников.");
        }

        Id = id;
        PostId = postId;
        Criteria = criteria;
        DateTime = dateTime;
        SecondGroupSlug = secondGroupSlug;
        OrganizerId = organizerId;
        Participants = participants;
        Prizes = prizes;
        Winners = new HashSet<Winner>();
    }

    public Guid Id { get; }
    public string PostId { get; }
    public Criteria Criteria { get; }
    public DateTime DateTime { get; }
    public string? SecondGroupSlug { get; }
    public Guid OrganizerId { get; }
    public IReadOnlySet<long> Participants { get; }
    public IReadOnlySet<Prize> Prizes { get; }
    public IReadOnlySet<Winner> Winners { get; private set; }

    public static Raffle Create(
        string postId,
        Criteria criteria,
        Guid organizerId,
        IEnumerable<long> participants,
        IEnumerable<Prize> prizes,
        string? secondGroupSlug = null)
    {
        var id = Guid.NewGuid();
        var dateTime = DateTime.UtcNow;

        return new Raffle(
            id,
            postId,
            criteria,
            dateTime,
            organizerId,
            participants.ToHashSet(),
            prizes.ToHashSet(),
            secondGroupSlug
        );
    }

    public void ChooseTheWinners()
    {
        if (Winners.Any())
        {
            throw new Exception("Победители уже определены.");
        }

        var prizeIds = new Stack<Guid>(Prizes.SelectMany(prize => Enumerable.Repeat(prize.Id, prize.Count)));
        var winners = Participants.OrderBy(_ => Guid.NewGuid()).Take(prizeIds.Count);

        foreach (var winner in winners)
        {
            Winners = new HashSet<Winner>(Winners) { new(winner, prizeIds.Pop()) };
        }
    }
}