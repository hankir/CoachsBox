using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    public partial class AddOtherStudentDocumentsTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "StudentDocuments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowCompetition",
                table: "StudentDocuments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowTraining",
                table: "StudentDocuments",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "EndDate_Day",
                table: "StudentDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EndDate_Year",
                table: "StudentDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndDate_Month_Name",
                table: "StudentDocuments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EndDate_Month_Number",
                table: "StudentDocuments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentContracts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<int>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Date_Month_Number = table.Column<int>(nullable: true),
                    Date_Month_Name = table.Column<string>(nullable: true),
                    Date_Year = table.Column<int>(nullable: true),
                    Date_Day = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentContracts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentContracts_StudentId",
                table: "StudentContracts",
                column: "StudentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentContracts");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "AllowCompetition",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "AllowTraining",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "EndDate_Day",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "EndDate_Year",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "EndDate_Month_Name",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "EndDate_Month_Number",
                table: "StudentDocuments");
        }
    }
}
