using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
  public partial class AddWhenEnrolledAndTrialTrainingCountToEnrolledStudent : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<byte>(
          name: "TrialTrainingCount",
          table: "EnrolledStudent",
          nullable: false,
          defaultValue: (byte)0);

      migrationBuilder.AddColumn<DateTime>(
          name: "WhenEnrolled",
          table: "EnrolledStudent",
          nullable: false,
          defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

      migrationBuilder.Sql(@"UPDATE enrolledstudent AS enrl
SET enrl.WhenEnrolled = COALESCE((
	SELECT MIN(evnt.WhenOccured) AS WhenOccured
	FROM coachingaccountingevents AS evnt
	LEFT JOIN studentaccounts AS acc ON evnt.AccountId = acc.Id
	WHERE evnt.GroupId = enrl.GroupId AND acc.StudentId = enrl.StudentId
	GROUP BY evnt.GroupId, acc.StudentId
), '2020-01-01 00:00:00.000000')");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "TrialTrainingCount",
          table: "EnrolledStudent");

      migrationBuilder.DropColumn(
          name: "WhenEnrolled",
          table: "EnrolledStudent");
    }
  }
}
