using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    public partial class OnDeleteGroupCascadeDeleteAccountingEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachingAccountingEvents_Groups_GroupId",
                table: "CoachingAccountingEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_CoachingAccountingEvents_Groups_GroupId",
                table: "CoachingAccountingEvents",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachingAccountingEvents_Groups_GroupId",
                table: "CoachingAccountingEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_CoachingAccountingEvents_Groups_GroupId",
                table: "CoachingAccountingEvents",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
