using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    public partial class RemoveRelationFromAccountEntryToGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountEntry_Groups_GroupId",
                table: "AccountEntry");

            migrationBuilder.DropIndex(
                name: "IX_AccountEntry_GroupId",
                table: "AccountEntry");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "AccountEntry");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AccountEntry",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AccountEntry");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "AccountEntry",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountEntry_GroupId",
                table: "AccountEntry",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountEntry_Groups_GroupId",
                table: "AccountEntry",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
