using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationService.src.AuthorizationService.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeparateUpdateGithubInfoComplexProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "github_id",
                table: "AspNetUsers",
                newName: "github_info_github_id");

            migrationBuilder.RenameColumn(
                name: "avatar_url",
                table: "AspNetUsers",
                newName: "github_info_avatar_url");

            migrationBuilder.AlterColumn<string>(
                name: "github_info_github_id",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "github_info_github_email",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "github_info_github_username",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "github_info_profile_url",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "github_info_github_email",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "github_info_github_username",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "github_info_profile_url",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "github_info_github_id",
                table: "AspNetUsers",
                newName: "github_id");

            migrationBuilder.RenameColumn(
                name: "github_info_avatar_url",
                table: "AspNetUsers",
                newName: "avatar_url");

            migrationBuilder.AlterColumn<string>(
                name: "github_id",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
