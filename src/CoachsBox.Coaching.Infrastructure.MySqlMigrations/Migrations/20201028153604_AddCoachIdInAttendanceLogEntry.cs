using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
  public partial class AddCoachIdInAttendanceLogEntry : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<int>(
          name: "CoachId",
          table: "AttendanceLogEntry",
          nullable: false,
          defaultValue: 0);

      migrationBuilder.Sql(@"
UPDATE attendancelogentry AS logEntry
SET coachId = (
	SELECT coachId
	FROM schedules AS s
	WHERE s.GroupId = (
		SELECT groupId
		FROM attendancelogs AS alogs
		WHERE alogs.Id = logEntry.AttendanceLogId))
");

      migrationBuilder.CreateIndex(
          name: "IX_AttendanceLogEntry_CoachId",
          table: "AttendanceLogEntry",
          column: "CoachId");

      migrationBuilder.AddForeignKey(
          name: "FK_AttendanceLogEntry_Coaches_CoachId",
          table: "AttendanceLogEntry",
          column: "CoachId",
          principalTable: "Coaches",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_AttendanceLogEntry_Coaches_CoachId",
          table: "AttendanceLogEntry");

      migrationBuilder.DropIndex(
          name: "IX_AttendanceLogEntry_CoachId",
          table: "AttendanceLogEntry");

      migrationBuilder.DropColumn(
          name: "CoachId",
          table: "AttendanceLogEntry");
    }
  }
}
