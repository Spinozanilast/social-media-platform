using AutoFixture;
using AutoFixture.Xunit2;

namespace AuthorizationService.Tests.Unit.CustomGenerators;

public class AutoDataAttribute<TFixtureCustomizer> : AutoDataAttribute where TFixtureCustomizer : IFixtureCustomizer
{
    public AutoDataAttribute() : base(fixtureFactory: () =>
    {
        var fixture = new Fixture();
        TFixtureCustomizer.Customize(fixture);
        return fixture;
    })
    {
    }
}