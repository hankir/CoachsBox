using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    public partial class AttendanceLogModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachingStaffMember_Branches_BranchId",
                table: "CoachingStaffMember");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrolledStudent_Groups_GroupId",
                table: "EnrolledStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingTime_Schedules_ScheduleId",
                table: "TrainingTime");

            migrationBuilder.CreateTable(
                name: "AttendanceLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceLogs_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceLogEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<int>(nullable: false),
                    Date_Month_Number = table.Column<int>(nullable: true),
                    Date_Month_Name = table.Column<string>(nullable: true),
                    Date_Year = table.Column<int>(nullable: true),
                    Date_Day = table.Column<byte>(nullable: true),
                    Start_Hours = table.Column<byte>(nullable: true),
                    Start_Minutes = table.Column<byte>(nullable: true),
                    End_Hours = table.Column<byte>(nullable: true),
                    End_Minutes = table.Column<byte>(nullable: true),
                    AbsenceReason_Reason = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AttendanceLogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceLogEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceLogEntry_AttendanceLogs_AttendanceLogId",
                        column: x => x.AttendanceLogId,
                        principalTable: "AttendanceLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceLogEntry_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogEntry_AttendanceLogId",
                table: "AttendanceLogEntry",
                column: "AttendanceLogId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogEntry_StudentId",
                table: "AttendanceLogEntry",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceLogs_GroupId",
                table: "AttendanceLogs",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoachingStaffMember_Branches_BranchId",
                table: "CoachingStaffMember",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnrolledStudent_Groups_GroupId",
                table: "EnrolledStudent",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingTime_Schedules_ScheduleId",
                table: "TrainingTime",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachingStaffMember_Branches_BranchId",
                table: "CoachingStaffMember");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrolledStudent_Groups_GroupId",
                table: "EnrolledStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingTime_Schedules_ScheduleId",
                table: "TrainingTime");

            migrationBuilder.DropTable(
                name: "AttendanceLogEntry");

            migrationBuilder.DropTable(
                name: "AttendanceLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_CoachingStaffMember_Branches_BranchId",
                table: "CoachingStaffMember",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EnrolledStudent_Groups_GroupId",
                table: "EnrolledStudent",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingTime_Schedules_ScheduleId",
                table: "TrainingTime",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
