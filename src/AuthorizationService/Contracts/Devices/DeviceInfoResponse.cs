namespace IdentityService.Contracts.Devices;

public record DeviceInfoResponse(
    string DeviceId,
    string DeviceName,
    string IpAdress,
    DateTime CreatedAt,
    DateTime Expires);