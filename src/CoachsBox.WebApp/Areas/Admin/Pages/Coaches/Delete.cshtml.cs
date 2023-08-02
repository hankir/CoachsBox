using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoachsBox.Coaching.CoachModel;
using CoachsBox.Coaching.Infrastructure;

namespace CoachsBox.WebApp.Areas.Admin.Pages.Coaches
{
    public class DeleteModel : PageModel
    {
        private readonly CoachsBox.Coaching.Infrastructure.CoachsBoxContext _context;

        public DeleteModel(CoachsBox.Coaching.Infrastructure.CoachsBoxContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Coach Coach { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Coach = await _context.Coaches
                .Include(c => c.Person).FirstOrDefaultAsync(m => m.Id == id);

            if (Coach == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Coach = await _context.Coaches.FindAsync(id);

            if (Coach != null)
            {
                _context.Coaches.Remove(Coach);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
