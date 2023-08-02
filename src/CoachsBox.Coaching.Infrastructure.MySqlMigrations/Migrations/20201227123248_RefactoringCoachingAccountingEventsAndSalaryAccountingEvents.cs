using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachsBox.Coaching.Infrastructure.MySqlMigrations.Migrations
{
    public partial class RefactoringCoachingAccountingEventsAndSalaryAccountingEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountingEventProcessLog_CoachingAccountingEvents_Accountin~",
                table: "AccountingEventProcessLog");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingEventResultingEntry_AccountEntry_AccountEntryId",
                table: "AccountingEventResultingEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountingEventResultingEntry_AccountingEventProcessLog_Proc~",
                table: "AccountingEventResultingEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_CoachingAccountingEvents_StudentAccounts_AccountId",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_CoachingAccountingEvents_Groups_GroupId",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_CoachingAccountingEvents_CoachingServiceAgreements_ServiceAg~",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_CoachingAccountingEvents_AgreementRegistry_AgreementRegistry~",
                table: "CoachingAccountingEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountingEventResultingEntry",
                table: "AccountingEventResultingEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountingEventProcessLog",
                table: "AccountingEventProcessLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoachingAccountingEvents",
                table: "CoachingAccountingEvents");

            migrationBuilder.RenameTable(
                name: "AccountingEventResultingEntry",
                newName: "StudentAccountingEventResultingEntries");

            migrationBuilder.RenameTable(
                name: "AccountingEventProcessLog",
                newName: "StudentAccountingEventProcessingState");

            migrationBuilder.RenameTable(
                name: "CoachingAccountingEvents",
                newName: "StudentAccountingEvents");

            migrationBuilder.RenameIndex(
                name: "IX_AccountingEventResultingEntry_ProcessingStateId",
                table: "StudentAccountingEventResultingEntries",
                newName: "IX_StudentAccountingEventResultingEntries_ProcessingStateId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountingEventResultingEntry_AccountEntryId",
                table: "StudentAccountingEventResultingEntries",
                newName: "IX_StudentAccountingEventResultingEntries_AccountEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_CoachingAccountingEvents_AgreementRegistryEntryId",
                table: "StudentAccountingEvents",
                newName: "IX_StudentAccountingEvents_AgreementRegistryEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_CoachingAccountingEvents_ServiceAgreementId",
                table: "StudentAccountingEvents",
                newName: "IX_StudentAccountingEvents_ServiceAgreementId");

            migrationBuilder.RenameIndex(
                name: "IX_CoachingAccountingEvents_GroupId",
                table: "StudentAccountingEvents",
                newName: "IX_StudentAccountingEvents_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_CoachingAccountingEvents_AccountId",
                table: "StudentAccountingEvents",
                newName: "IX_StudentAccountingEvents_AccountId");

            migrationBuilder.AddColumn<int>(
                name: "SalaryDebtAccountingEventId",
                table: "SalaryCalculation",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Salaries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAccountingEventResultingEntries",
                table: "StudentAccountingEventResultingEntries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAccountingEventProcessingState",
                table: "StudentAccountingEventProcessingState",
                column: "AccountingEventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAccountingEvents",
                table: "StudentAccountingEvents",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SalaryAccountingEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventType_Name = table.Column<string>(nullable: true),
                    WhenOccured = table.Column<DateTime>(nullable: false),
                    WhenNoticed = table.Column<DateTime>(nullable: false),
                    SalaryId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    CoachId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    TrainingCost_Quantity = table.Column<decimal>(nullable: true),
                    TrainingCost_Currency = table.Column<int>(nullable: true),
                    TrainingCount = table.Column<int>(nullable: true),
                    ProcessedInSalaryId = table.Column<int>(nullable: true),
                    AccountId = table.Column<int>(nullable: true),
                    CalculationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryAccountingEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryAccountingEvents_Salaries_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "Salaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalaryAccountingEvents_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalaryAccountingEvents_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalaryAccountingEvents_Salaries_ProcessedInSalaryId",
                        column: x => x.ProcessedInSalaryId,
                        principalTable: "Salaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalaryAccountingEvents_SalaryCalculation_CalculationId",
                        column: x => x.CalculationId,
                        principalTable: "SalaryCalculation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalaryAccountingEventProcessingState",
                columns: table => new
                {
                    AccountingEventId = table.Column<int>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false),
                    WhenProcessed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryAccountingEventProcessingState", x => x.AccountingEventId);
                    table.ForeignKey(
                        name: "FK_SalaryAccountingEventProcessingState_SalaryAccountingEvents_~",
                        column: x => x.AccountingEventId,
                        principalTable: "SalaryAccountingEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalaryAccountingEventResultingEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountEntryId = table.Column<int>(nullable: false),
                    ProcessingStateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryAccountingEventResultingEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryAccountingEventResultingEntries_AccountEntry_AccountEn~",
                        column: x => x.AccountEntryId,
                        principalTable: "AccountEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryAccountingEventResultingEntries_SalaryAccountingEventP~",
                        column: x => x.ProcessingStateId,
                        principalTable: "SalaryAccountingEventProcessingState",
                        principalColumn: "AccountingEventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryCalculation_SalaryDebtAccountingEventId",
                table: "SalaryCalculation",
                column: "SalaryDebtAccountingEventId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccounts_GroupId",
                table: "GroupAccounts",
                column: "GroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAccountingEventResultingEntries_AccountEntryId",
                table: "SalaryAccountingEventResultingEntries",
                column: "AccountEntryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAccountingEventResultingEntries_ProcessingStateId",
                table: "SalaryAccountingEventResultingEntries",
                column: "ProcessingStateId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAccountingEvents_SalaryId",
                table: "SalaryAccountingEvents",
                column: "SalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAccountingEvents_CoachId",
                table: "SalaryAccountingEvents",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAccountingEvents_GroupId",
                table: "SalaryAccountingEvents",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAccountingEvents_ProcessedInSalaryId",
                table: "SalaryAccountingEvents",
                column: "ProcessedInSalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryAccountingEvents_CalculationId",
                table: "SalaryAccountingEvents",
                column: "CalculationId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupAccounts_Groups_GroupId",
                table: "GroupAccounts",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryCalculation_SalaryAccountingEvents_SalaryDebtAccountin~",
                table: "SalaryCalculation",
                column: "SalaryDebtAccountingEventId",
                principalTable: "SalaryAccountingEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAccountingEventProcessingState_StudentAccountingEvent~",
                table: "StudentAccountingEventProcessingState",
                column: "AccountingEventId",
                principalTable: "StudentAccountingEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAccountingEventResultingEntries_AccountEntry_AccountE~",
                table: "StudentAccountingEventResultingEntries",
                column: "AccountEntryId",
                principalTable: "AccountEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAccountingEventResultingEntries_StudentAccountingEven~",
                table: "StudentAccountingEventResultingEntries",
                column: "ProcessingStateId",
                principalTable: "StudentAccountingEventProcessingState",
                principalColumn: "AccountingEventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAccountingEvents_AgreementRegistry_AgreementRegistryE~",
                table: "StudentAccountingEvents",
                column: "AgreementRegistryEntryId",
                principalTable: "AgreementRegistry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAccountingEvents_StudentAccounts_AccountId",
                table: "StudentAccountingEvents",
                column: "AccountId",
                principalTable: "StudentAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAccountingEvents_Groups_GroupId",
                table: "StudentAccountingEvents",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAccountingEvents_CoachingServiceAgreements_ServiceAgr~",
                table: "StudentAccountingEvents",
                column: "ServiceAgreementId",
                principalTable: "CoachingServiceAgreements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupAccounts_Groups_GroupId",
                table: "GroupAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_SalaryCalculation_SalaryAccountingEvents_SalaryDebtAccountin~",
                table: "SalaryCalculation");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccountingEventProcessingState_StudentAccountingEvent~",
                table: "StudentAccountingEventProcessingState");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccountingEventResultingEntries_AccountEntry_AccountE~",
                table: "StudentAccountingEventResultingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccountingEventResultingEntries_StudentAccountingEven~",
                table: "StudentAccountingEventResultingEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccountingEvents_AgreementRegistry_AgreementRegistryE~",
                table: "StudentAccountingEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccountingEvents_StudentAccounts_AccountId",
                table: "StudentAccountingEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccountingEvents_Groups_GroupId",
                table: "StudentAccountingEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccountingEvents_CoachingServiceAgreements_ServiceAgr~",
                table: "StudentAccountingEvents");

            migrationBuilder.DropTable(
                name: "SalaryAccountingEventResultingEntries");

            migrationBuilder.DropTable(
                name: "SalaryAccountingEventProcessingState");

            migrationBuilder.DropTable(
                name: "SalaryAccountingEvents");

            migrationBuilder.DropIndex(
                name: "IX_SalaryCalculation_SalaryDebtAccountingEventId",
                table: "SalaryCalculation");

            migrationBuilder.DropIndex(
                name: "IX_GroupAccounts_GroupId",
                table: "GroupAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAccountingEventResultingEntries",
                table: "StudentAccountingEventResultingEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAccountingEventProcessingState",
                table: "StudentAccountingEventProcessingState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAccountingEvents",
                table: "StudentAccountingEvents");

            migrationBuilder.DropColumn(
                name: "SalaryDebtAccountingEventId",
                table: "SalaryCalculation");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Salaries");

            migrationBuilder.RenameTable(
                name: "StudentAccountingEventResultingEntries",
                newName: "AccountingEventResultingEntry");

            migrationBuilder.RenameTable(
                name: "StudentAccountingEventProcessingState",
                newName: "AccountingEventProcessLog");

            migrationBuilder.RenameTable(
                name: "StudentAccountingEvents",
                newName: "CoachingAccountingEvents");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAccountingEventResultingEntries_ProcessingStateId",
                table: "AccountingEventResultingEntry",
                newName: "IX_AccountingEventResultingEntry_ProcessingStateId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAccountingEventResultingEntries_AccountEntryId",
                table: "AccountingEventResultingEntry",
                newName: "IX_AccountingEventResultingEntry_AccountEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAccountingEvents_ServiceAgreementId",
                table: "CoachingAccountingEvents",
                newName: "IX_CoachingAccountingEvents_ServiceAgreementId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAccountingEvents_GroupId",
                table: "CoachingAccountingEvents",
                newName: "IX_CoachingAccountingEvents_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAccountingEvents_AccountId",
                table: "CoachingAccountingEvents",
                newName: "IX_CoachingAccountingEvents_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAccountingEvents_AgreementRegistryEntryId",
                table: "CoachingAccountingEvents",
                newName: "IX_CoachingAccountingEvents_AgreementRegistryEntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountingEventResultingEntry",
                table: "AccountingEventResultingEntry",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountingEventProcessLog",
                table: "AccountingEventProcessLog",
                column: "AccountingEventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoachingAccountingEvents",
                table: "CoachingAccountingEvents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingEventProcessLog_CoachingAccountingEvents_Accountin~",
                table: "AccountingEventProcessLog",
                column: "AccountingEventId",
                principalTable: "CoachingAccountingEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingEventResultingEntry_AccountEntry_AccountEntryId",
                table: "AccountingEventResultingEntry",
                column: "AccountEntryId",
                principalTable: "AccountEntry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountingEventResultingEntry_AccountingEventProcessLog_Proc~",
                table: "AccountingEventResultingEntry",
                column: "ProcessingStateId",
                principalTable: "AccountingEventProcessLog",
                principalColumn: "AccountingEventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoachingAccountingEvents_StudentAccounts_AccountId",
                table: "CoachingAccountingEvents",
                column: "AccountId",
                principalTable: "StudentAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoachingAccountingEvents_Groups_GroupId",
                table: "CoachingAccountingEvents",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoachingAccountingEvents_CoachingServiceAgreements_ServiceAg~",
                table: "CoachingAccountingEvents",
                column: "ServiceAgreementId",
                principalTable: "CoachingServiceAgreements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoachingAccountingEvents_AgreementRegistry_AgreementRegistry~",
                table: "CoachingAccountingEvents",
                column: "AgreementRegistryEntryId",
                principalTable: "AgreementRegistry",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
