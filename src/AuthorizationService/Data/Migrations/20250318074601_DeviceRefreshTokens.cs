using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityService.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeviceRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "RefreshToken",
                newName: "Expires");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "RefreshToken",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "RefreshToken",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
                table: "RefreshToken",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "RefreshToken",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "Expires",
                table: "RefreshToken",
                newName: "ExpiryDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RefreshToken",
                newName: "Created");
        }
    }
}
