using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
  public partial class ChangeStudentDocumentRelationTypeToMany : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        name: "FK_StudentDocuments_Students_StudentId",
        table: "StudentDocuments");

      migrationBuilder.DropIndex(
          name: "IX_StudentDocuments_StudentId",
          table: "StudentDocuments");

      migrationBuilder.CreateIndex(
          name: "IX_StudentDocuments_StudentId",
          table: "StudentDocuments",
          column: "StudentId");

      migrationBuilder.AddForeignKey(
        name: "FK_StudentDocuments_Students_StudentId",
        table: "StudentDocuments",
        column: "StudentId",
        principalTable: "Students",
        principalColumn: "Id",
        onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
        name: "FK_StudentDocuments_Students_StudentId",
        table: "StudentDocuments");

      migrationBuilder.DropIndex(
          name: "IX_StudentDocuments_StudentId",
          table: "StudentDocuments");

      migrationBuilder.CreateIndex(
          name: "IX_StudentDocuments_StudentId",
          table: "StudentDocuments",
          column: "StudentId",
          unique: true);

      migrationBuilder.AddForeignKey(
        name: "FK_StudentDocuments_Students_StudentId",
        table: "StudentDocuments",
        column: "StudentId",
        principalTable: "Students",
        principalColumn: "Id",
        onDelete: ReferentialAction.Cascade);
    }
  }
}
