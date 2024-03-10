using AutoFixture;
using VKRaffles.Domain.Entities;

namespace VKRaffles.Domain.Tests.Tools;

public class PrizeCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var id = fixture.Create<Guid>();
        var name = fixture.GenerateString(Prize.MaxNameLength);
        var count = fixture.GenerateIntFromRange(Prize.MinCount, Prize.MaxCount);
        var raffleId = fixture.Create<Guid>();

        fixture.Customize<Prize>(c => c.FromFactory(() =>
            new Prize(id, name, count, raffleId)));
    }
}