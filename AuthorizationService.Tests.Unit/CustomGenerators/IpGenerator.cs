using AutoFixture;

namespace AuthorizationService.Tests.Unit.CustomGenerators;

public class IpGenerator : IFixtureCustomizer
{
    private static int GenerateIpPart() => Random.Shared.Next(0, 255);

    public static void Customize(IFixture fixture) =>
        fixture.Register(() =>
            new CustomString(string.Join(":",
                [GenerateIpPart(), GenerateIpPart(), GenerateIpPart(), GenerateIpPart()])));
}

public class CustomString(string s);