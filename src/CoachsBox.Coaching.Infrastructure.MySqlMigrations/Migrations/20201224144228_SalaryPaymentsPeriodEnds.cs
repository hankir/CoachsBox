using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    public partial class SalaryPaymentsPeriodEnds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PaymentsPeriodEnding_Day",
                table: "Salaries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentsPeriodEnding_Year",
                table: "Salaries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentsPeriodEnding_Month_Name",
                table: "Salaries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentsPeriodEnding_Month_Number",
                table: "Salaries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentsPeriodEnding_Day",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "PaymentsPeriodEnding_Year",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "PaymentsPeriodEnding_Month_Name",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "PaymentsPeriodEnding_Month_Number",
                table: "Salaries");
        }
    }
}
