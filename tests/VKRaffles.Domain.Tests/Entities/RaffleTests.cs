using AutoFixture;
using FluentAssertions;
using VKRaffles.Domain.Entities;
using VKRaffles.Domain.Tests.Tools;

namespace VKRaffles.Domain.Tests.Entities;

public class RaffleTests
{
    private readonly Fixture _fixture;

    public RaffleTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new PrizeCustomization());
    }

    [Fact]
    public void Init_CorrectParams_SuccessInit()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var raffle = new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        raffle.Id.Should().Be(id);
        raffle.PostId.Should().Be(postId);
        raffle.Criteria.Should().Be(criteria);
        raffle.DateTime.Should().Be(dateTime);
        raffle.OrganizerId.Should().Be(organizerId);
        raffle.Participants.Should().Equal(participants);
        raffle.Prizes.Should().Equal(prizes);
        raffle.SecondGroupSlug.Should().Be(secondGroupSlug);
    }

    [Fact]
    public void Init_EmptyGuidId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(id));
    }

    [Fact]
    public void Init_InvalidPostId_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.Create<string>();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(postId));
    }

    [Fact]
    public void Init_DefaultCriteria_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = default(Criteria);
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(criteria));
    }

    [Fact]
    public void Init_DefaultDateTime_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = default(DateTime);
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(dateTime));
    }

    [Fact]
    public void Init_EmptyGuidOrganizerId_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = Guid.Empty;
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(organizerId));
    }

    [Theory]
    [InlineData(null!)]
    [InlineData(default(HashSet<long>))]
    public void Init_NullOrEmptyParticipants_ThrowsArgumentException(IReadOnlySet<long> participants)
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(participants));
    }

    [Fact]
    public void Init_TooManyParticipants_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(Raffle.MaxParticipantsCount + 1).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(participants));
    }

    [Theory]
    [InlineData(null!)]
    [InlineData(default(HashSet<Prize>))]
    public void Init_NullOrEmptyPrizes_ThrowsArgumentException(IReadOnlySet<Prize> prizes)
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var participants = _fixture.CreateMany<long>(1).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(prizes));
    }

    [Fact]
    public void Init_TooManyPrizes_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount + 1).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(prizes));
    }

    [Fact]
    public void Init_SecondGroupSlugIsNullButCriteriaSelected_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Subscription | Criteria.SecondGroupSubscription;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        string? secondGroupSlug = null;

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Init_TooLongSecondGroupSlug_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength + 1);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(secondGroupSlug));
    }

    [Fact]
    public void Init_CriteriaSecondGroupSubscriptionSelectedButSubscriptionNotSelected_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.SecondGroupSubscription;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(criteria));
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Init_NegativeOrZeroInParticipants_ThrowsArgumentException(long participant)
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        participants.Add(participant);
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(participants));
    }

    [Fact]
    public void Init_PrizesCountBiggerThenParticipantsCount_ThrowsAggregateException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count) - 1).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var action = () =>
            new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        action.Should().Throw<AggregateException>();
    }

    [Fact]
    public void Create_CorrectParams_SuccessCreateAndReturn()
    {
        // Arrange
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);

        // Act
        var raffle = Raffle.Create(postId, criteria, organizerId, participants, prizes, secondGroupSlug);

        // Assert
        raffle.Id.Should().NotBeEmpty();
        raffle.PostId.Should().Be(postId);
        raffle.Criteria.Should().Be(criteria);
        raffle.DateTime.Should().NotBe(default);
        raffle.OrganizerId.Should().Be(organizerId);
        raffle.Participants.Should().Equal(participants);
        raffle.Prizes.Should().Equal(prizes);
        raffle.SecondGroupSlug.Should().Be(secondGroupSlug);
    }

    [Fact]
    public void ChooseTheWinners_SuccessWinnersChosen()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);
        var raffle = new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);
        var prizesCount = raffle.Prizes.Sum(p => p.Count);

        // Act
        raffle.ChooseTheWinners();

        // Assert
        raffle.Winners.Should().HaveCount(prizesCount);
    }

    [Fact]
    public void ChooseTheWinners_WinnersAlreadyChosen_ThrowsAggregateException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var postId = _fixture.GenerateRafflePostId();
        var criteria = Criteria.Like | Criteria.Repost;
        var dateTime = _fixture.Create<DateTime>();
        var organizerId = _fixture.Create<Guid>();
        var prizes = _fixture.CreateMany<Prize>(Raffle.MaxPrizesCount).ToHashSet();
        var participants = _fixture.GenerateLongRange(prizes.Sum(p => p.Count)).ToHashSet();
        var secondGroupSlug = _fixture.GenerateString(Raffle.MaxSecondGroupSlugLength);
        var raffle = new Raffle(id, postId, criteria, dateTime, organizerId, participants, prizes, secondGroupSlug);
        var prizesCount = raffle.Prizes.Sum(p => p.Count);
        raffle.ChooseTheWinners();

        // Act
        var action = () => raffle.ChooseTheWinners();

        // Assert
        action.Should().Throw<AggregateException>();
    }
}