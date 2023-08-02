using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.WebApp.Migrations.BackgroundServiceDb
{
    public partial class CreateBackgroundServiceDbContextScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceInfos",
                columns: table => new
                {
                    ServiceId = table.Column<string>(nullable: false),
                    RunInterval = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceInfos", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceEvents",
                columns: table => new
                {
                    ServiceId = table.Column<string>(nullable: false),
                    UtcLastRun = table.Column<DateTime>(nullable: false),
                    Result = table.Column<int>(nullable: false),
                    FailureReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceEvents", x => new { x.ServiceId, x.UtcLastRun });
                    table.ForeignKey(
                        name: "FK_ServiceEvents_ServiceInfos_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "ServiceInfos",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceEvents");

            migrationBuilder.DropTable(
                name: "ServiceInfos");
        }
    }
}
