using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CareerBuilderModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CareerSteps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProgressPercentage",
                table: "CareerSteps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CareerStepId",
                table: "CareerPlanSkills",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CareerPlanSkills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CareerPlans",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "CareerQuizAttemptId",
                table: "CareerPlans",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgressPercentage",
                table: "CareerPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CareerQuizAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswersJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerQuizAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CareerQuizAttempts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareerPlanSkills_CareerStepId",
                table: "CareerPlanSkills",
                column: "CareerStepId");

            migrationBuilder.CreateIndex(
                name: "IX_CareerPlans_CareerQuizAttemptId",
                table: "CareerPlans",
                column: "CareerQuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_CareerQuizAttempts_UserId",
                table: "CareerQuizAttempts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareerPlans_CareerQuizAttempts_CareerQuizAttemptId",
                table: "CareerPlans",
                column: "CareerQuizAttemptId",
                principalTable: "CareerQuizAttempts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CareerPlanSkills_CareerSteps_CareerStepId",
                table: "CareerPlanSkills",
                column: "CareerStepId",
                principalTable: "CareerSteps",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareerPlans_CareerQuizAttempts_CareerQuizAttemptId",
                table: "CareerPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_CareerPlanSkills_CareerSteps_CareerStepId",
                table: "CareerPlanSkills");

            migrationBuilder.DropTable(
                name: "CareerQuizAttempts");

            migrationBuilder.DropIndex(
                name: "IX_CareerPlanSkills_CareerStepId",
                table: "CareerPlanSkills");

            migrationBuilder.DropIndex(
                name: "IX_CareerPlans_CareerQuizAttemptId",
                table: "CareerPlans");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CareerSteps");

            migrationBuilder.DropColumn(
                name: "ProgressPercentage",
                table: "CareerSteps");

            migrationBuilder.DropColumn(
                name: "CareerStepId",
                table: "CareerPlanSkills");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CareerPlanSkills");

            migrationBuilder.DropColumn(
                name: "CareerQuizAttemptId",
                table: "CareerPlans");

            migrationBuilder.DropColumn(
                name: "ProgressPercentage",
                table: "CareerPlans");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "CareerPlans",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
