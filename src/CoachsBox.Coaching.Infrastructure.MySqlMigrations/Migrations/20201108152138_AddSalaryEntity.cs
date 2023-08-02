using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    public partial class AddSalaryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PeriodBeginning_Month_Number = table.Column<int>(nullable: true),
                    PeriodBeginning_Month_Name = table.Column<string>(nullable: true),
                    PeriodBeginning_Year = table.Column<int>(nullable: true),
                    PeriodBeginning_Day = table.Column<byte>(nullable: true),
                    PeriodEnding_Month_Number = table.Column<int>(nullable: true),
                    PeriodEnding_Month_Name = table.Column<string>(nullable: true),
                    PeriodEnding_Year = table.Column<int>(nullable: true),
                    PeriodEnding_Day = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryCalculation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount_Quantity = table.Column<decimal>(nullable: true),
                    Amount_Currency = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    SalaryId = table.Column<int>(nullable: true),
                    CoachId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    TrainingCount = table.Column<int>(nullable: true),
                    TrainingCost_Quantity = table.Column<decimal>(nullable: true),
                    TrainingCost_Currency = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryCalculation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryCalculation_Salary_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "Salary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryCalculation_SalaryId",
                table: "SalaryCalculation",
                column: "SalaryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryCalculation");

            migrationBuilder.DropTable(
                name: "Salary");
        }
    }
}
