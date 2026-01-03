using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserProfileSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfileSkills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProficiencyLevel = table.Column<int>(type: "int", nullable: false),
                    AcquisitionMethod = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfileSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfileSkills_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileSkills_DisplayOrder",
                table: "UserProfileSkills",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileSkills_IsFeatured",
                table: "UserProfileSkills",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileSkills_ProficiencyLevel",
                table: "UserProfileSkills",
                column: "ProficiencyLevel");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileSkills_SkillId",
                table: "UserProfileSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileSkills_UserProfile_Skill_Unique",
                table: "UserProfileSkills",
                columns: new[] { "UserProfileId", "SkillId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileSkills_UserProfileId",
                table: "UserProfileSkills",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfileSkills");
        }
    }
}
