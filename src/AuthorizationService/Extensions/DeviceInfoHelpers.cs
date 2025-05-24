using Authentication.Configuration.Options;
using AuthorizationService.Contracts;

namespace AuthorizationService.Extensions;

public static class DeviceInfoHelpers
{
    public static DeviceInfo GetDeviceInfo(this HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var deviceName = context.GetDeviceUserAgent();

        return new DeviceInfo(deviceName, ipAddress ?? string.Empty);
    }

    public static string GetDeviceUserAgent(this HttpContext context)
    {
        return context.Request.Headers.UserAgent.ToString();
    }
}