using AutoFixture;
using FluentAssertions;
using VKRaffles.Domain.Entities;
using VKRaffles.Domain.Tests.Tools;

namespace VKRaffles.Domain.Tests.Entities;

public class ParticipantTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Init_CorrectParams_SuccessInit()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var vkId = _fixture.Create<long>();
        var raffleId = _fixture.Create<Guid>();

        // Act
        var participant = new Participant(id, vkId, raffleId);

        // Assert
        participant.Id.Should().Be(id);
        participant.VkId.Should().Be(vkId);
        participant.RaffleId.Should().Be(raffleId);
    }

    [Fact]
    public void Init_EmptyGuidId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;
        var vkId = _fixture.Create<long>();
        var raffleId = _fixture.Create<Guid>();

        // Act
        var action = () => new Participant(id, vkId, raffleId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(id));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Init_NegativeOrZeroVkId_ThrowsArgumentException(int vkId)
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var raffleId = _fixture.Create<Guid>();

        // Act
        var action = () => new Participant(id, vkId, raffleId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(vkId));
    }

    [Fact]
    public void Init_EmptyGuidRaffleId_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var vkId = _fixture.Create<long>();
        var raffleId = Guid.Empty;

        // Act
        var action = () => new Participant(id, vkId, raffleId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(raffleId));
    }

    [Fact]
    public void Create_CorrectParams_SuccessCreateAndReturn()
    {
        // Arrange
        var vkId = _fixture.Create<long>();
        var raffleId = _fixture.Create<Guid>();

        // Act
        var participant = Participant.Create(vkId, raffleId);

        // Assert
        participant.VkId.Should().Be(vkId);
        participant.RaffleId.Should().Be(raffleId);
    }
}