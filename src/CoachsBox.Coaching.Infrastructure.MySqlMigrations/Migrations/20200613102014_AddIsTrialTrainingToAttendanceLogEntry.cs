using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    public partial class AddIsTrialTrainingToAttendanceLogEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTrialTraining",
                table: "AttendanceLogEntry",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTrialTraining",
                table: "AttendanceLogEntry");
        }
    }
}
