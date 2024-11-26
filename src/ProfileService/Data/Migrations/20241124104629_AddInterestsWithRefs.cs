using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProfileService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInterestsWithRefs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "References",
                table: "Profiles",
                type: "varchar(80)[]",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "Interest",
                columns: table => new
                {
                    InterestId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interest", x => x.InterestId);
                });

            migrationBuilder.CreateTable(
                name: "InterestProfile",
                columns: table => new
                {
                    InterestsInterestId = table.Column<int>(type: "integer", nullable: false),
                    ProfilesUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestProfile", x => new { x.InterestsInterestId, x.ProfilesUserId });
                    table.ForeignKey(
                        name: "FK_InterestProfile_Interest_InterestsInterestId",
                        column: x => x.InterestsInterestId,
                        principalTable: "Interest",
                        principalColumn: "InterestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterestProfile_Profiles_ProfilesUserId",
                        column: x => x.ProfilesUserId,
                        principalTable: "Profiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestProfile_ProfilesUserId",
                table: "InterestProfile",
                column: "ProfilesUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestProfile");

            migrationBuilder.DropTable(
                name: "Interest");

            migrationBuilder.DropColumn(
                name: "References",
                table: "Profiles");
        }
    }
}
