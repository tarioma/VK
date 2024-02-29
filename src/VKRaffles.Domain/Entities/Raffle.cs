using Ardalis.GuardClauses;
using GuardClauses;

namespace VKRaffles.Domain.Entities;

public record Raffle
{
    public const int MaxPrizesCount = 10;
    public const int MaxSecondGroupSlugLength = 33;

    private const string PostIdPattern = @"^-\d{1,10}_\d{1,10}$";

    private readonly HashSet<Winner> _winners;

    public Raffle(
        Guid id,
        string postId,
        Criteria criteria,
        DateTime dateTime,
        Guid organizerId,
        IEnumerable<Participant> participants,
        IEnumerable<Prize> prizes,
        string? secondGroupSlug)
    {
        Guard.Against.Default(id);
        Guard.Against.Null(postId);
        Guard.Against.InvalidFormat(postId, nameof(postId), PostIdPattern);
        Guard.Against.Default(organizerId);

        if (secondGroupSlug is not null)
        {
            Guard.Against.StringTooLong(secondGroupSlug, MaxSecondGroupSlugLength);
        }

        Guard.Against.Default(criteria);
        ValidateCriteria(criteria);

        var participantsArray = participants as Participant[] ?? participants.ToArray();
        Guard.Against.NullOrEmpty(participantsArray);

        var prizesArray = prizes as Prize[] ?? prizes.ToHashSet().ToArray();
        Guard.Against.NullOrEmpty(prizesArray);
        Guard.Against.OutOfRange(prizesArray.Length, nameof(prizes), 1, MaxPrizesCount);

        ValidatePrizesCountWithParticipantsCount(participantsArray, prizesArray);

        Id = id;
        PostId = postId;
        Criteria = criteria;
        DateTime = dateTime;
        SecondGroupSlug = secondGroupSlug?.Trim();
        OrganizerId = organizerId;
        Participants = participantsArray.ToHashSet();
        Prizes = prizesArray.ToHashSet();
        _winners = new HashSet<Winner>();
    }

    public Guid Id { get; }
    public string PostId { get; }
    public Criteria Criteria { get; }
    public DateTime DateTime { get; }
    public string? SecondGroupSlug { get; }
    public Guid OrganizerId { get; }
    public IReadOnlyCollection<Participant> Participants { get; }
    public IReadOnlyCollection<Prize> Prizes { get; }
    public IReadOnlyCollection<Winner> Winners => _winners;

    public static Raffle Create(
        string postId,
        Criteria criteria,
        Guid organizerId,
        IEnumerable<Participant> participants,
        IEnumerable<Prize> prizes,
        string? secondGroupSlug = null)
    {
        var id = Guid.NewGuid();
        var dateTime = DateTime.UtcNow;

        return new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);
    }

    public void ChooseTheWinners()
    {
        var prizeIds = new Stack<Guid>(Prizes.SelectMany(prize => Enumerable.Repeat(prize.Id, prize.Count)));
        var random = new Random();
        var winners = Participants.OrderBy(_ => random.Next())
            .Take(prizeIds.Count)
            .ToArray();

        foreach (var winner in winners)
        {
            _winners.Add(new Winner(winner.Id, prizeIds.Pop()));
        }
    }

    private static void ValidateCriteria(Criteria criteria)
    {
        if ((criteria & Criteria.Subscription) == 0 && (criteria & Criteria.SecondGroupSubscription) != 0)
        {
            throw new ArgumentException("Второе сообщество не может быть выбрано без основного.", nameof(criteria));
        }
    }

    private static void ValidatePrizesCountWithParticipantsCount(IEnumerable<Participant> participants,
        IEnumerable<Prize> prizes)
    {
        var prizesCount = prizes.Sum(p => p.Count);

        if (prizesCount > participants.Count())
        {
            throw new AggregateException("Количество призов не может быть больше количества участников.");
        }
    }
}