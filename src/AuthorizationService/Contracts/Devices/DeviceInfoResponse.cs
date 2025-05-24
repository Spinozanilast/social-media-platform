namespace AuthorizationService.Contracts.Devices;

public record DeviceInfoResponse(
    string DeviceName,
    string IpAdress,
    DateTime CreatedAt,
    DateTime Expires);