using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuthorizationService.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixRefreshTokensSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "idx_refreshTokens_deviceName",
                table: "refresh_token");

            migrationBuilder.DropIndex(
                name: "ix_refresh_token_user_id",
                table: "refresh_token");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "refresh_token",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_refresh_token",
                table: "refresh_token",
                columns: new[] { "user_id", "id" });

            migrationBuilder.CreateIndex(
                name: "idx_refreshTokens_deviceName",
                table: "refresh_token",
                column: "device_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_refresh_token",
                table: "refresh_token");

            migrationBuilder.DropIndex(
                name: "idx_refreshTokens_deviceName",
                table: "refresh_token");

            migrationBuilder.DropColumn(
                name: "id",
                table: "refresh_token");

            migrationBuilder.AddPrimaryKey(
                name: "idx_refreshTokens_deviceName",
                table: "refresh_token",
                column: "device_name");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_token_user_id",
                table: "refresh_token",
                column: "user_id");
        }
    }
}
