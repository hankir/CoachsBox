using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.WebApp.Migrations
{
    public partial class CoachsBoxWebAppUserReferToPersonId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoachId",
                table: "AspNetUsers",
                newName: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "AspNetUsers",
                newName: "CoachId");
        }
    }
}
