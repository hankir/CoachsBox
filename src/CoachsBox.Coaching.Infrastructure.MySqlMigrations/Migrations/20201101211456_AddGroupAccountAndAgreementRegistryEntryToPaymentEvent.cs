using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
  public partial class AddGroupAccountAndAgreementRegistryEntryToPaymentEvent : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      // Удаляем все проведенные платежи, для того чтобы провести их снова,
      // но уже с дополнительной проводкой поступлений на счета групп.
      migrationBuilder.Sql(@"
-- Удаление записей счета, созданных по событию Payment.
DELETE FROM accountentry
WHERE accountentry.EntryType_Name = 'Payment';
	
-- Отметка событий платеж не обработанными.
UPDATE accountingeventprocesslog
SET accountingeventprocesslog.IsProcessed = 0
WHERE accountingeventprocesslog.AccountingEventId IN (
	SELECT coachingaccountingevents.Id FROM coachingaccountingevents
	WHERE coachingaccountingevents.EventType_Name = 'Payment');
");

      migrationBuilder.AddColumn<int>(
          name: "AgreementRegistryEntryId",
          table: "CoachingAccountingEvents",
          nullable: true);

      migrationBuilder.Sql(@"
UPDATE coachingaccountingevents
SET AgreementRegistryEntryId = (
	SELECT Id
	FROM agreementregistry
	WHERE agreementregistry.GroupId = coachingaccountingevents.GroupId)
WHERE coachingaccountingevents.EventType_Name = 'Payment'
");

      migrationBuilder.AddColumn<int>(
          name: "GroupAccountId",
          table: "AgreementRegistry",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "GroupAccountId",
          table: "AccountEntry",
          nullable: true);

      migrationBuilder.CreateTable(
          name: "GroupAccounts",
          columns: table => new
          {
            Id = table.Column<int>(nullable: false)
                  .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            GroupId = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_GroupAccounts", x => x.Id);
          });

      migrationBuilder.CreateIndex(
          name: "IX_CoachingAccountingEvents_AgreementRegistryEntryId",
          table: "CoachingAccountingEvents",
          column: "AgreementRegistryEntryId");

      migrationBuilder.CreateIndex(
          name: "IX_AgreementRegistry_GroupAccountId",
          table: "AgreementRegistry",
          column: "GroupAccountId");

      migrationBuilder.CreateIndex(
          name: "IX_AccountEntry_GroupAccountId",
          table: "AccountEntry",
          column: "GroupAccountId");

      migrationBuilder.AddForeignKey(
          name: "FK_AccountEntry_GroupAccounts_GroupAccountId",
          table: "AccountEntry",
          column: "GroupAccountId",
          principalTable: "GroupAccounts",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
          name: "FK_AgreementRegistry_GroupAccounts_GroupAccountId",
          table: "AgreementRegistry",
          column: "GroupAccountId",
          principalTable: "GroupAccounts",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);

      migrationBuilder.AddForeignKey(
          name: "FK_CoachingAccountingEvents_AgreementRegistry_AgreementRegistry~",
          table: "CoachingAccountingEvents",
          column: "AgreementRegistryEntryId",
          principalTable: "AgreementRegistry",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);

      migrationBuilder.Sql(@"
INSERT INTO groupaccounts (GroupId)
SELECT agreementregistry.GroupId FROM agreementregistry
LEFT JOIN groupaccounts ON agreementregistry.GroupId = groupaccounts.GroupId
WHERE groupaccounts.Id IS NULL;

UPDATE agreementregistry
SET agreementregistry.GroupAccountId = (
	SELECT groupaccounts.Id FROM groupaccounts
	WHERE groupaccounts.GroupId = agreementregistry.GroupId
);
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_AccountEntry_GroupAccounts_GroupAccountId",
          table: "AccountEntry");

      migrationBuilder.DropForeignKey(
          name: "FK_AgreementRegistry_GroupAccounts_GroupAccountId",
          table: "AgreementRegistry");

      migrationBuilder.DropForeignKey(
          name: "FK_CoachingAccountingEvents_AgreementRegistry_AgreementRegistry~",
          table: "CoachingAccountingEvents");

      migrationBuilder.DropTable(
          name: "GroupAccounts");

      migrationBuilder.DropIndex(
          name: "IX_CoachingAccountingEvents_AgreementRegistryEntryId",
          table: "CoachingAccountingEvents");

      migrationBuilder.DropIndex(
          name: "IX_AgreementRegistry_GroupAccountId",
          table: "AgreementRegistry");

      migrationBuilder.DropIndex(
          name: "IX_AccountEntry_GroupAccountId",
          table: "AccountEntry");

      migrationBuilder.DropColumn(
          name: "AgreementRegistryEntryId",
          table: "CoachingAccountingEvents");

      migrationBuilder.DropColumn(
          name: "GroupAccountId",
          table: "AgreementRegistry");

      migrationBuilder.DropColumn(
          name: "GroupAccountId",
          table: "AccountEntry");
    }
  }
}
