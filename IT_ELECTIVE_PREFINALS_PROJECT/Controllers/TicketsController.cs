using IT_ELECTIVE_PREFINALS_PROJECT.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IT_ELECTIVE_PREFINALS_PROJECT.Controllers;

public class TicketsController : Controller
{
    private readonly AppDbContext _context;

    public TicketsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var tickets = await _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .ToListAsync();
        return View(tickets);
    }
}