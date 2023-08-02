using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    public partial class AddDebtsTrainingCountAmountToIssuedDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryCalculation_Salary_SalaryId",
                table: "SalaryCalculation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Salary",
                table: "Salary");

            migrationBuilder.RenameTable(
                name: "Salary",
                newName: "Salaries");

            migrationBuilder.AddColumn<int>(
                name: "DebtTrainingCount",
                table: "SalaryCalculation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SalaryCalculation",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AmountToIssued_Currency",
                table: "SalaryCalculation",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountToIssued_Quantity",
                table: "SalaryCalculation",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Salaries",
                table: "Salaries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryCalculation_Salaries_SalaryId",
                table: "SalaryCalculation",
                column: "SalaryId",
                principalTable: "Salaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryCalculation_Salaries_SalaryId",
                table: "SalaryCalculation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Salaries",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "DebtTrainingCount",
                table: "SalaryCalculation");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SalaryCalculation");

            migrationBuilder.DropColumn(
                name: "AmountToIssued_Currency",
                table: "SalaryCalculation");

            migrationBuilder.DropColumn(
                name: "AmountToIssued_Quantity",
                table: "SalaryCalculation");

            migrationBuilder.RenameTable(
                name: "Salaries",
                newName: "Salary");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Salary",
                table: "Salary",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryCalculation_Salary_SalaryId",
                table: "SalaryCalculation",
                column: "SalaryId",
                principalTable: "Salary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
