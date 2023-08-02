using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.WebApp.Migrations.BackgroundServiceDb
{
  public partial class AddNextStartToServiceInfoAndRemoveRunInterval : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "RunInterval",
          table: "ServiceInfos");

      migrationBuilder.AddColumn<DateTime>(
          name: "NextStart",
          table: "ServiceInfos",
          nullable: false,
          defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

      migrationBuilder.Sql(@"DELETE FROM serviceevents;
DELETE FROM serviceinfos
WHERE serviceinfos.ServiceId = 'CoachsBox.WebApp.Jobs.AccrualServiceWorker';");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "NextStart",
          table: "ServiceInfos");

      migrationBuilder.AddColumn<TimeSpan>(
          name: "RunInterval",
          table: "ServiceInfos",
          type: "time(6)",
          nullable: false,
          defaultValue: new TimeSpan(0, 0, 0, 0, 0));
    }
  }
}
