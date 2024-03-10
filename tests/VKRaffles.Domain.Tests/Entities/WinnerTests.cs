using AutoFixture;
using FluentAssertions;
using VKRaffles.Domain.Entities;

namespace VKRaffles.Domain.Tests.Entities;

public class WinnerTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Init_CorrectParams_SuccessInit()
    {
        // Arrange
        var participantVkId = _fixture.Create<long>();
        var prizeId = _fixture.Create<Guid>();

        // Act
        var participant = new Winner(participantVkId, prizeId);

        // Assert
        participant.ParticipantVkId.Should().Be(participantVkId);
        participant.PrizeId.Should().Be(prizeId);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Init_NegativeOrZeroParticipantVkId_ThrowsArgumentException(long participantVkId)
    {
        // Arrange
        var prizeId = _fixture.Create<Guid>();

        // Act
        var action = () => new Winner(participantVkId, prizeId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(participantVkId));
    }

    [Fact]
    public void Init_EmptyGuidPrizeId_ThrowsArgumentException()
    {
        // Arrange
        var participantVkId = _fixture.Create<long>();
        var prizeId = Guid.Empty;

        // Act
        var action = () => new Winner(participantVkId, prizeId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(prizeId));
    }
}