using MessagingService.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MessagingService.Entities;

[EntityTypeConfiguration(typeof(ChatsFacesConfiguration))]
public class ChatsFace
{
    public int UserId { get; init; }

    public bool IsDefault { get; set; } = false;

    public string Username { get; set; } = string.Empty;

    public string? Status { get; set; } = string.Empty;
}