using System.Threading.Tasks;
using CoachsBox.Coaching.Accounting.StudentAccountModel;
using CoachsBox.Infrastructure;

namespace CoachsBox.Coaching.Infrastructure
{
  public class StudentAccountRepository : EfRepository<StudentAccount, CoachsBoxContext>, IStudentAccountRepository
  {
    public async override Task<StudentAccount> GetByIdAsync(int id)
    {
      var account = await context.Set<StudentAccount>().FindAsync(id);
      if (account != null)
        await this.context.Entry(account).Collection(studentAccount => studentAccount.Entries).LoadAsync();
      return account;
    }
    public StudentAccountRepository(CoachsBoxContext dbContext)
      : base(dbContext)
    {
    }
  }
}
