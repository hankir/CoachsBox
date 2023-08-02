using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    public partial class AddTrainingPeriodInAccountEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "CoachingAccountingEvents",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "TrainingDate_Day",
                table: "CoachingAccountingEvents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainingDate_Year",
                table: "CoachingAccountingEvents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Month_Name",
                table: "CoachingAccountingEvents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Month_Number",
                table: "CoachingAccountingEvents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingDate_Month_Name",
                table: "CoachingAccountingEvents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainingDate_Month_Number",
                table: "CoachingAccountingEvents",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "EndOfTraining_Hours",
                table: "CoachingAccountingEvents",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "EndOfTraining_Minutes",
                table: "CoachingAccountingEvents",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "StartOfTraining_Hours",
                table: "CoachingAccountingEvents",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "StartOfTraining_Minutes",
                table: "CoachingAccountingEvents",
                nullable: true);

      migrationBuilder.Sql(@"
UPDATE coachingaccountingevents
SET
	TrainingDate_Day = DAYOFMONTH(WhenOccured),
	TrainingDate_Month_Number = MONTH(WhenOccured),
	TrainingDate_Month_Name = MONTHNAME(WhenOccured),
	TrainingDate_Year = YEAR(WhenOccured),
	StartOfTraining_Hours = HOUR(WhenOccured),
	StartOfTraining_Minutes = MINUTE(WhenOccured),
	EndOfTraining_Hours = HOUR(WhenOccured) + 1,
	EndOfTraining_Minutes = MINUTE(WhenOccured)
WHERE EventType_Name = 'PersonalTrainingAccrual' AND EndOfTraining_Hours IS NULL;

UPDATE coachingaccountingevents
SET
	Month_Name = MONTHNAME(WhenOccured),
	Month_Number = MONTH(WhenOccured),
	Year = YEAR(WhenOccured)
WHERE EventType_Name = 'Accrual' AND YEAR IS NULL;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropColumn(
                name: "TrainingDate_Day",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropColumn(
                name: "TrainingDate_Year",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropColumn(
                name: "Month_Name",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropColumn(
                name: "Month_Number",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropColumn(
                name: "TrainingDate_Month_Name",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropColumn(
                name: "TrainingDate_Month_Number",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropColumn(
                name: "EndOfTraining_Hours",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropColumn(
                name: "EndOfTraining_Minutes",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropColumn(
                name: "StartOfTraining_Hours",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropColumn(
                name: "StartOfTraining_Minutes",
                table: "CoachingAccountingEvents");
        }
    }
}
