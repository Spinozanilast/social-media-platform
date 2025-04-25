using AutoFixture;

namespace AuthorizationService.Tests.Unit.CustomGenerators;

public interface IFixtureCustomizer
{
    static abstract void Customize(IFixture fixture);
}