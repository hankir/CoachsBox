using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Core;
using CoachsBox.WebApp.Areas.Identity.Data;
using CoachsBox.WebApp.Areas.Identity.Services;
using CoachsBox.WebApp.Pages.Facade;
using Coravel.Invocable;
using Coravel.Queuing.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CoachsBox.WebApp.Areas.Identity.Pages.Account.Manage
{
  public class SummaryGroupsAttendanceLogByMonthModel : PageModel
  {
    private readonly IQueue coravelQueue;
    private readonly UserManager<CoachsBoxWebAppUser> userManager;

    public SummaryGroupsAttendanceLogByMonthModel(
      IQueue queue,
      UserManager<CoachsBoxWebAppUser> userManager)
    {
      this.coravelQueue = queue;
      this.userManager = userManager;
    }

    public IActionResult OnPostAsync(DateTime reportPeriod, string email)
    {
      var userId = this.userManager.GetUserId(this.User);
      this.coravelQueue.QueueInvocableWithPayload<SendSummaryGroupsAttendanceLogByMonthMail, SendSummaryGroupsAttendanceLogByMonthMailPayload>(
        new SendSummaryGroupsAttendanceLogByMonthMailPayload()
        {
          Period = reportPeriod != default ? reportPeriod : Watch.Now.Date,
          UserId = userId,
          CoachId = this.User.GetCoachId(),
          Email = email
        });

      return this.RedirectToPage();
    }
  }

  public class SendSummaryGroupsAttendanceLogByMonthMailPayload
  {
    public DateTime Period { get; set; }

    public string UserId { get; set; }

    public int CoachId { get; set; }

    public string Email { get; set; }
  }

  public class SendSummaryGroupsAttendanceLogByMonthMail : IInvocable, IInvocableWithPayload<SendSummaryGroupsAttendanceLogByMonthMailPayload>
  {
    private readonly IEmailSenderWithAttachments emailSender;
    private readonly IAttendanceLogServiceFacade attendanceLogServiceFacade;
    private readonly IGroupManagmentServiceFacade groupManagmentService;
    private readonly UserManager<CoachsBoxWebAppUser> userManager;
    private readonly ILogger<SendSummaryGroupsAttendanceLogByMonthMail> logger;

    public SendSummaryGroupsAttendanceLogByMonthMail(
      IAttendanceLogServiceFacade attendanceLogServiceFacade,
      IGroupManagmentServiceFacade groupManagmentService,
      IEmailSenderWithAttachments emailSender,
      UserManager<CoachsBoxWebAppUser> userManager,
      ILogger<SendSummaryGroupsAttendanceLogByMonthMail> logger)
    {
      this.attendanceLogServiceFacade = attendanceLogServiceFacade;
      this.groupManagmentService = groupManagmentService;
      this.emailSender = emailSender;
      this.userManager = userManager;
      this.logger = logger;
    }

    public SendSummaryGroupsAttendanceLogByMonthMailPayload Payload { get; set; }

    public async Task Invoke()
    {
      var sw = Stopwatch.StartNew();
      string email = string.Empty;
      var period = this.Payload.Period;
      var reportName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(period.Month);
      try
      {
        this.logger.LogDebug("Start build report {ReportName}", nameof(SendSummaryGroupsAttendanceLogByMonthMail));

        List<int> groupIds;
        var user = await this.userManager.FindByIdAsync(this.Payload.UserId);
        email = string.IsNullOrWhiteSpace(this.Payload.Email) ? user.Email : this.Payload.Email;
        if (await this.userManager.IsInRoleAsync(user, CoachsBoxWebAppRole.Coach))
          groupIds = this.groupManagmentService.ListGroupsByCoach(this.Payload.CoachId).OrderBy(g => g.Name).Select(g => g.Id).ToList();
        else
          groupIds = this.groupManagmentService.ListGroups().OrderBy(g => g.Name).Select(g => g.Id).ToList();

        using (var reportStream = new MemoryStream())
        {
          using (var report = SpreadsheetDocument.Create(reportStream, SpreadsheetDocumentType.Workbook))
          {
            var worksheetPart = InsertWorksheetPart(report, reportName);
            uint currentRowIndex = 1;
            foreach (var groupId in groupIds)
            {
              var attendanceLog = await this.attendanceLogServiceFacade.ViewAttendanceLog(groupId, period.Month, period.Year, true);

              var trainingCount = attendanceLog.ColumnsTitle.Count;
              Cell cell = InsertCellInWorksheet("A", currentRowIndex, worksheetPart);
              cell.CellValue = new CellValue($"{attendanceLog.AttendanceLog.GroupName}");
              cell.DataType = CellValues.String;

              cell = InsertCellInWorksheet("B", currentRowIndex, worksheetPart);
              cell.CellValue = new CellValue($"Кол-во тренировок: {trainingCount}");
              cell.DataType = CellValues.String;

              currentRowIndex++;
              cell = InsertCellInWorksheet("A", currentRowIndex, worksheetPart);
              cell.CellValue = new CellValue($"Ученик (Общее кол-во: {attendanceLog.RowsTitle.Count})");
              cell.DataType = CellValues.String;

              cell = InsertCellInWorksheet("B", currentRowIndex, worksheetPart);
              cell.CellValue = new CellValue("Был");
              cell.DataType = CellValues.String;

              cell = InsertCellInWorksheet("C", currentRowIndex, worksheetPart);
              cell.CellValue = new CellValue("Не был");
              cell.DataType = CellValues.String;

              cell = InsertCellInWorksheet("D", currentRowIndex, worksheetPart);
              cell.CellValue = new CellValue("Болел");
              cell.DataType = CellValues.String;

              cell = InsertCellInWorksheet("E", currentRowIndex, worksheetPart);
              cell.CellValue = new CellValue("Нет отметки");
              cell.DataType = CellValues.String;

              currentRowIndex++;
              foreach (var student in attendanceLog.RowsTitle)
              {
                var studentAttendanceLog = attendanceLog.AttendanceLog.Entries.Where(entry => entry.StudentId == student.StudentId).ToList();
                cell = InsertCellInWorksheet("A", currentRowIndex, worksheetPart);
                cell.CellValue = new CellValue(student.StudentFullName);
                cell.DataType = CellValues.String;

                cell = InsertCellInWorksheet("B", currentRowIndex, worksheetPart);
                var existCount = studentAttendanceLog.Count(a => a.AbsenceReason == null);
                cell.CellValue = new CellValue(existCount.ToString());
                cell.DataType = CellValues.Number;

                cell = InsertCellInWorksheet("C", currentRowIndex, worksheetPart);
                var notExistCount = studentAttendanceLog.Count(a => a.AbsenceReason == Absence.WithoutValidExcuse.Reason);
                cell.CellValue = new CellValue(notExistCount.ToString());
                cell.DataType = CellValues.Number;

                cell = InsertCellInWorksheet("D", currentRowIndex, worksheetPart);
                var siknessCount = studentAttendanceLog.Count(a => a.AbsenceReason == Absence.Sickness.Reason);
                cell.CellValue = new CellValue(siknessCount.ToString());
                cell.DataType = CellValues.Number;

                cell = InsertCellInWorksheet("E", currentRowIndex, worksheetPart);
                var noAttendCount = trainingCount - studentAttendanceLog.Count;
                cell.CellValue = new CellValue(noAttendCount.ToString());
                cell.DataType = CellValues.Number;
                currentRowIndex++;
              }

              currentRowIndex += 2;
            }
            worksheetPart.Worksheet.Save();
          }

          reportStream.Position = 0;
          await this.emailSender.SendEmailAsync(
            email,
            $"Посещаемость за {reportName}",
            $"Отчет сформирован",
            new Dictionary<string, Stream>()
            {
              { "Report.xlsx", reportStream }
            });
        }
        sw.Stop();
        this.logger.LogDebug("Report {ReportName} finished and send to {Email} at {Elapsed}", nameof(SendSummaryGroupsAttendanceLogByMonthMail), email, sw.Elapsed);
      }
      catch (Exception ex)
      {
        sw.Stop();
        this.logger.LogError(ex, "Report {ReportName} failed at {Elapsed}", nameof(SendSummaryGroupsAttendanceLogByMonthMail), sw.Elapsed);
        try
        {
          if (!string.IsNullOrWhiteSpace(email))
          {
            await this.emailSender.SendEmailAsync(
              email,
              $"Посещаемость за {reportName} - ошибка отчета",
              $"Не удалось сформировать отчет.<br />Обратитесь в поддержку.");
          }
        }
        catch (Exception sendEmailEx)
        {
          this.logger.LogError(sendEmailEx, "Can not send email about report {ReportName} failed", nameof(SendSummaryGroupsAttendanceLogByMonthMail));
        }
      }
    }

    private static WorksheetPart InsertWorksheetPart(SpreadsheetDocument report, string sheetName)
    {
      // Add a WorkbookPart to the document.
      var workbookpart = report.AddWorkbookPart();
      workbookpart.Workbook = new Workbook();

      // Add a WorksheetPart to the WorkbookPart.
      var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
      worksheetPart.Worksheet = new Worksheet(new SheetData());

      // Add Sheets to the Workbook.
      var sheets = report.WorkbookPart.Workbook.AppendChild(new Sheets());

      // Append a new worksheet and associate it with the workbook.
      var sheet = new Sheet()
      {
        Id = report.WorkbookPart.GetIdOfPart(worksheetPart),
        SheetId = 1,
        Name = sheetName
      };
      sheets.Append(sheet);
      return worksheetPart;
    }

    private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
    {
      Worksheet worksheet = worksheetPart.Worksheet;
      SheetData sheetData = worksheet.GetFirstChild<SheetData>();
      string cellReference = columnName + rowIndex;

      // If the worksheet does not contain a row with the specified row index, insert one.
      Row row;
      if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
      {
        row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
      }
      else
      {
        row = new Row() { RowIndex = rowIndex };
        sheetData.Append(row);
      }

      // If there is not a cell with the specified column name, insert one.  
      if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
      {
        return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
      }
      else
      {
        // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
        Cell refCell = null;
        foreach (Cell cell in row.Elements<Cell>())
        {
          if (cell.CellReference.Value.Length == cellReference.Length)
          {
            if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
            {
              refCell = cell;
              break;
            }
          }
        }

        Cell newCell = new Cell() { CellReference = cellReference };
        row.InsertBefore(newCell, refCell);
        return newCell;
      }
    }
  }
}
