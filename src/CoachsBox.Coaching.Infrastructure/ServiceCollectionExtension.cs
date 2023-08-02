using System.Reflection;
using CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel;
using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.GroupAccountEntryModel;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Coaching.Accounting.SalaryAccountingEventModel;
using CoachsBox.Coaching.Accounting.SalaryModel;
using CoachsBox.Coaching.Accounting.StudentAccountEntryModel;
using CoachsBox.Coaching.Accounting.StudentAccountingEventModel;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Coaching.Accounting.StudentAccountPostingRuleModel;
using CoachsBox.Coaching.Accounting.StudentContractModel;
using CoachsBox.Coaching.Application;
using CoachsBox.Coaching.Application.DomainEventHandlers;
using CoachsBox.Coaching.Application.Impl;
using CoachsBox.Coaching.Application.Queries;
using CoachsBox.Coaching.Application.Queries.Impl;
using CoachsBox.Coaching.AttendanceLogModel;
using CoachsBox.Coaching.BranchModel;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.GroupModel;
using CoachsBox.Coaching.Infrastructure.Accounting;
using CoachsBox.Coaching.Infrastructure.Coaching;
using CoachsBox.Coaching.PersonModel;
using CoachsBox.Coaching.ScheduleModel;
using CoachsBox.Coaching.StudentDocumentModel;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CoachsBox.Coaching.Infrastructure
{
  public static class ServiceCollectionExtension
  {
    public static IServiceCollection AddCoachingInfrastructure(this IServiceCollection services)
    {
      // Репозитории
      //   Coaching
      services.AddScoped(typeof(ICoachRepository), typeof(CoachRepository));
      services.AddScoped(typeof(IPersonRepository), typeof(PersonRepository));
      services.AddScoped(typeof(IBranchRepository), typeof(BranchRepository));
      services.AddScoped(typeof(IGroupRepository), typeof(GroupRepository));
      services.AddScoped(typeof(IScheduleRepository), typeof(ScheduleRepository));
      services.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));
      services.AddScoped(typeof(IAttendanceLogRepository), typeof(AttendanceLogRepository));
      services.AddScoped(typeof(IStudentDocumentRepository), typeof(StudentDocumentRepository));
      //   Accounting
      services.AddScoped(typeof(IAgreementRegistryEntryRepository), typeof(AgreementRegistryEntryRepository));
      services.AddScoped(typeof(ICoachingServiceAgreementRepository), typeof(CoachingServiceAgreementRepository));
      services.AddScoped(typeof(IStudentAccountingEventRepository), typeof(StudentAccountingEventRepository));
      services.AddScoped(typeof(IStudentAccountRepository), typeof(StudentAccountRepository));
      services.AddScoped(typeof(IStudentAccountEntryRepository), typeof(StudentAccountEntryRepository));
      services.AddScoped(typeof(IStudentAccountPostingRuleRepository), typeof(StudentAccountPostingRuleRepository));
      services.AddScoped(typeof(IStudentContractRepository), typeof(StudentContractRepository));
      services.AddScoped(typeof(IGroupAccountRepository), typeof(GroupAccountRepository));
      services.AddScoped(typeof(IGroupAccountEntryRepository), typeof(GroupAccountEntryRepository));
      services.AddScoped(typeof(ISalaryRepository), typeof(SalaryRepository));
      services.AddScoped(typeof(ISalaryAccountingEventRepository), typeof(SalaryAccountingEventRepository));

      // Unit of work
      services.AddScoped(typeof(IUnitOfWork), typeof(CoachsBoxUnitOfWork));

      // Службы
      //   Coaching
      services.AddScoped(typeof(IAdministrativeService), typeof(AdministrativeService));
      services.AddScoped(typeof(IGroupManagmentService), typeof(GroupManagmentService));
      services.AddScoped(typeof(ISchedulingService), typeof(SchedulingService));
      services.AddScoped(typeof(ICoachQueryService), typeof(CoachQueryService));
      services.AddScoped(typeof(IAttendanceQueryService), typeof(AttendanceQueryService));
      services.AddScoped(typeof(IGroupQueryService), typeof(GroupQueryService));
      services.AddScoped(typeof(IStudentQueryService), typeof(StudentQueryService));

      //   Accounting
      services.AddScoped(typeof(IAccrualService), typeof(AccrualService));
      services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
      services.AddScoped(typeof(IAccountingService), typeof(AccountingService));
      services.AddScoped(typeof(ISalaryQueryService), typeof(SalaryQueryService));
      services.AddScoped(typeof(IStudentAccountQueryService), typeof(StudentAccountQueryService));

      // Запросы
      services.AddScoped(typeof(IStudentDocumentsQueries), typeof(StudentDocumentsQueries));

      // Команды и обработчики.
      // Доменные события и обработчики.
      services.AddMediatR(new[] {
        typeof(Student).GetTypeInfo().Assembly,
        typeof(CreatedStudentEventHandler).GetTypeInfo().Assembly
      });

      return services;
    }
  }
}
