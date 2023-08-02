using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoachsBox.Core.Interfaces;

namespace CoachsBox.Coaching.GroupModel
{
  public interface IGroupRepository : IAsyncRepository<Group>
  {
  }
}
