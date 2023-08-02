using CoachsBox.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachsBox.Coaching.Infrastructure
{
  public class CoachsBoxContextSeed
  {
    public async Task SeedAsync(CoachsBoxContext context, bool migrate = false)
    {
      if (migrate)
        context.Database.Migrate();

      await this.SeedOverrideAsync(context);
      await context.SaveChangesAsync();
    }

    protected virtual async Task SeedOverrideAsync(CoachsBoxContext context)
    {
      await Task.CompletedTask;
    }
  }
}
