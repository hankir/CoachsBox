using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachsBox.Coaching.Application;
using CoachsBox.Coaching.Application.Commands;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.WebApp.Areas.Admin.Facade;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Payments
{
  public class IndexModel : PageModel
  {
    private readonly IStudentRepository studentRepository;
    private readonly IGroupRepository groupRepository;
    private readonly IPaymentService paymentService;
    private readonly IMediator mediator;

    public IndexModel(
      IStudentRepository studentRepository,
      IGroupRepository groupRepository,
      IPaymentService paymentService,
      IMediator mediator)
    {
      this.studentRepository = studentRepository;
      this.groupRepository = groupRepository;
      this.paymentService = paymentService;
      this.Commands = new List<CreatePaymentCommand>();
      this.mediator = mediator;
    }

    [BindProperty]
    public string StudentName { get; set; }

    [BindProperty]
    public CreatePaymentCommand CreatePaymentCommand { get; set; }

    public List<CreatePaymentCommand> Commands { get; private set; }

    public void OnGet(string studentName)
    {
      this.StudentName = studentName;
      if (!string.IsNullOrWhiteSpace(this.StudentName))
      {
        var nameParts = studentName.Trim().Split(' ', 3);
        string surname = nameParts.Length >= 1 ? nameParts[0] : null;
        string name = nameParts.Length >= 2 ? nameParts[1] : null;
        string patronymic = nameParts.Length >= 3 ? nameParts[2] : null;

        var findByNameSpec = new FindStudentsByNameSpecification(new PersonName(surname, name, patronymic)).WithPerson();
        var students = this.studentRepository.ListAsync(findByNameSpec).Result;

        var groupByStudentSpec = new GroupByStudentsIdsSpecification(students.Select(student => student.Id).ToList());
        var groups = this.groupRepository.ListAsync(groupByStudentSpec).Result;

        foreach (var student in students)
        {
          var studentGroups = groups.Where(g => g.EnrolledStudents.Any(enrolled => enrolled.StudentId == student.Id));
          if (studentGroups.Any())
          {
            foreach (var group in studentGroups)
            {
              this.Commands.Add(new CreatePaymentCommand()
              {
                StudentId = student.Id,
                StudentName = student.Person.Name.FullName(),
                GroupId = group.Id,
                GroupName = group.Name
              });
            }
          }
          else
          {
            this.Commands.Add(new CreatePaymentCommand()
            {
              StudentId = student.Id,
              StudentName = student.Person.Name.FullName()
            });
          }
        }
      }
    }

    public IActionResult OnPost()
    {
      // PRG паттерн https://en.wikipedia.org/wiki/Post/Redirect/Get.
      return this.RedirectToPage("Index", new { this.StudentName });
    }

    public async Task<IActionResult> OnPostRegisterPayment()
    {
      if (this.ModelState.IsValid)
      {
        var amount = this.CreatePaymentCommand.Amount;
        var whenOccured = this.User.FromUserTime(this.CreatePaymentCommand.WhenOccured);
        var studentId = this.CreatePaymentCommand.StudentId;
        var groupId = this.CreatePaymentCommand.GroupId;

        await this.mediator.Send(new RegisterPaymentCommand(amount, whenOccured, studentId, groupId));
      }

      return this.RedirectToPage("Index", new { this.StudentName });
    }
  }
}
