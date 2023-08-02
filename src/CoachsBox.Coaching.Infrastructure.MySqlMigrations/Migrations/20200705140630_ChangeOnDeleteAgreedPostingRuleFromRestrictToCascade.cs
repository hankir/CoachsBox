using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    public partial class ChangeOnDeleteAgreedPostingRuleFromRestrictToCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgreedPostingRule_PostingRule_PostingRuleId",
                table: "AgreedPostingRule");

            migrationBuilder.AddForeignKey(
                name: "FK_AgreedPostingRule_PostingRule_PostingRuleId",
                table: "AgreedPostingRule",
                column: "PostingRuleId",
                principalTable: "PostingRule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgreedPostingRule_PostingRule_PostingRuleId",
                table: "AgreedPostingRule");

            migrationBuilder.AddForeignKey(
                name: "FK_AgreedPostingRule_PostingRule_PostingRuleId",
                table: "AgreedPostingRule",
                column: "PostingRuleId",
                principalTable: "PostingRule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
