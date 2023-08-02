using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoachsBox.Coaching.GroupModel
{
  public interface IGroupQueryService
  {
    Task<string> GetName(int groupId);

    Task<IReadOnlyDictionary<int, string>> ListNames(int[] groupIds);
  }
}
