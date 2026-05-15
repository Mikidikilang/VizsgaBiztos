using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScorePercent",
                table: "ExamResults");

            migrationBuilder.RenameColumn(
                name: "CratedAt",
                table: "Sheets",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "OptionD",
                table: "Questions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswers",
                table: "ExamResults",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Score",
                table: "ExamResults",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalQuestions",
                table: "ExamResults",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "i7YvyaDQyUd7EV9YPv1RK6FbKQ7Z8L5RHkJbUfhO9dI=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionD",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "TotalQuestions",
                table: "ExamResults");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Sheets",
                newName: "CratedAt");

            migrationBuilder.AddColumn<double>(
                name: "ScorePercent",
                table: "ExamResults",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "admin123");
        }
    }
}
