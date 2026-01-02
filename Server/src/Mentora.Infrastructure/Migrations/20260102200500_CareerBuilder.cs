using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CareerBuilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareerPlanSkills_Skills_SkillId",
                table: "CareerPlanSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_CareerQuizAttempts_AspNetUsers_UserId",
                table: "CareerQuizAttempts");

            migrationBuilder.DropIndex(
                name: "IX_CareerQuizAttempts_UserId",
                table: "CareerQuizAttempts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CareerSteps");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CareerQuizAttempts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CareerQuizAttempts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "CareerQuizAttempts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "CareerPlans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_CareerPlanSkills_Skills_SkillId",
                table: "CareerPlanSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareerPlanSkills_Skills_SkillId",
                table: "CareerPlanSkills");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CareerSteps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "CareerQuizAttempts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CareerQuizAttempts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CareerQuizAttempts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "CareerPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CareerQuizAttempts_UserId",
                table: "CareerQuizAttempts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareerPlanSkills_Skills_SkillId",
                table: "CareerPlanSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CareerQuizAttempts_AspNetUsers_UserId",
                table: "CareerQuizAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
