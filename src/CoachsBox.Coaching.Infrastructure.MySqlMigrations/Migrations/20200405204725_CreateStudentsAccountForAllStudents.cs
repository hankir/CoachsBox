using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
  public partial class CreateStudentsAccountForAllStudents : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.Sql(@$"
INSERT INTO studentaccounts (StudentId)
SELECT s.Id AS StudentId FROM students AS s
LEFT JOIN studentaccounts AS a ON s.Id = a.StudentId
WHERE a.Id IS NULL;
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
  }
}
