using AutoFixture;
using FluentAssertions;
using VKRaffles.Domain.Entities;
using VKRaffles.Domain.Tests.Tools;

namespace VKRaffles.Domain.Tests.Entities;

public class PrizeTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void Init_CorrectParams_SuccessInit()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var name = _fixture.GenerateString(Prize.MaxNameLength);
        var count = _fixture.GenerateIntFromRange(Prize.MinCount, Prize.MaxCount);
        var raffleId = _fixture.Create<Guid>();

        // Act
        var prize = new Prize(id, name, count, raffleId);

        // Assert
        prize.Id.Should().Be(id);
        prize.Name.Should().Be(name);
        prize.Count.Should().Be(count);
        prize.RaffleId.Should().Be(raffleId);
    }

    [Fact]
    public void Init_EmptyGuidId_ThrowsArgumentException()
    {
        // Arrange
        var id = Guid.Empty;
        var name = _fixture.GenerateString(Prize.MaxNameLength);
        var count = _fixture.GenerateIntFromRange(Prize.MinCount, Prize.MaxCount);
        var raffleId = _fixture.Create<Guid>();

        // Act
        var action = () => new Prize(id, name, count, raffleId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(id));
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData(" ")]
    public void Init_InvalidName_ThrowsArgumentException(string name)
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var count = _fixture.GenerateIntFromRange(Prize.MinCount, Prize.MaxCount);
        var raffleId = _fixture.Create<Guid>();

        // Act
        var action = () => new Prize(id, name, count, raffleId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(name));
    }

    [Fact]
    public void Init_TooLongName_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var name = _fixture.GenerateString(Prize.MaxNameLength + 1);
        var count = _fixture.GenerateIntFromRange(Prize.MinCount, Prize.MaxCount);
        var raffleId = _fixture.Create<Guid>();

        // Act
        var action = () => new Prize(id, name, count, raffleId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(name));
    }

    [Fact]
    public void Init_TooLowCount_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var name = _fixture.GenerateString(Prize.MaxNameLength);
        var count = Prize.MinCount - 1;
        var raffleId = _fixture.Create<Guid>();

        // Act
        var action = () => new Prize(id, name, count, raffleId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(count));
    }

    [Fact]
    public void Init_TooHighCount_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var name = _fixture.GenerateString(Prize.MaxNameLength);
        var count = Prize.MaxCount + 1;
        var raffleId = _fixture.Create<Guid>();

        // Act
        var action = () => new Prize(id, name, count, raffleId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(count));
    }

    [Fact]
    public void Init_EmptyGuidRaffleId_ThrowsArgumentException()
    {
        // Arrange
        var id = _fixture.Create<Guid>();
        var name = _fixture.GenerateString(Prize.MaxNameLength);
        var count = _fixture.GenerateIntFromRange(Prize.MinCount, Prize.MaxCount);
        var raffleId = Guid.Empty;

        // Act
        var action = () => new Prize(id, name, count, raffleId);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithParameterName(nameof(raffleId));
    }

    [Fact]
    public void Create_CorrectParams_SuccessCreateAndReturn()
    {
        // Arrange
        var name = _fixture.GenerateString(Prize.MaxNameLength);
        var count = _fixture.GenerateIntFromRange(Prize.MinCount, Prize.MaxCount);
        var raffleId = _fixture.Create<Guid>();

        // Act
        var prize = Prize.Create(name, count, raffleId);

        // Assert
        prize.Name.Should().Be(name);
        prize.Count.Should().Be(count);
        prize.RaffleId.Should().Be(raffleId);
    }
}