using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoachsBox.Coaching.Infrastructure;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.StudentModel;
using MediatR;
using Xunit;
using Xunit.Abstractions;

namespace CoachsBox.IntegrationTests.DomainEvents
{
  public class CoachingDomainEventsTests : IntegrationTestsBase
  {
    private int createStudentEvent;
    private int studentId;

    [Fact]
    public void CreateStudentEvent()
    {
      using (var context = this.CreateContext())
      {
        var person = new Person(new PersonName("Шупкин", "Мупкин"));
        var student = new Student(person, "test");
        var studentRepository = new StudentRepository(context);
        studentRepository.AddAsync(student).Wait();
        context.SaveChanges();
      }

      Assert.Equal(1, this.createStudentEvent);
      Assert.Equal(1, this.studentId);
    }

    protected override IMediator GetMediator()
    {
      return new Mediator(new ServiceFactory((serviceType) =>
      {
        if (serviceType == typeof(IEnumerable<INotificationHandler<CreatedStudentEvent>>))
          return new List<INotificationHandler<CreatedStudentEvent>>()
          {
            new TestEventHandler<CreatedStudentEvent>((domainEvent) =>
            {
              this.studentId = domainEvent.Student.Id;
              this.createStudentEvent++;
            })
          };
        return null;
      }));
    }

    public CoachingDomainEventsTests(ITestOutputHelper output) : base(output)
    {
    }
  }

  public class TestEventHandler<T> : INotificationHandler<T> where T : INotification
  {
    private readonly Action<T> action;

    public TestEventHandler(Action<T> action)
    {
      this.action = action;
    }

    public Task Handle(T notification, CancellationToken cancellationToken)
    {
      this.action(notification);
      return Task.CompletedTask;
    }
  }
}
