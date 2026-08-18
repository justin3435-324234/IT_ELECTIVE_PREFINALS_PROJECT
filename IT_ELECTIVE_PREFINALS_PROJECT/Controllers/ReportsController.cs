using IT_ELECTIVE_PREFINALS_PROJECT.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace IT_ELECTIVE_PREFINALS_PROJECT.Controllers;

public class ReportsController : Controller
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalTickets = await _context.Tickets.CountAsync();
        ViewBag.UnassignedTicketsCount = await _context.Tickets
            .Where(t => !t.TicketAssignments.Any())
            .CountAsync();
        ViewBag.TotalEmployees = await _context.Employees.CountAsync();

        return View();
    }

    public async Task<IActionResult> Workload()
    {
        var workload = await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.TicketAssignments)
                .ThenInclude(ta => ta.Ticket)
                    .ThenInclude(t => t.Status)
            .ToListAsync();

        return View(workload);
    }

    public async Task<IActionResult> Unassigned()
    {
        var unassignedTickets = await _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .Where(t => !t.TicketAssignments.Any())
            .ToListAsync();

        return View(unassignedTickets);
    }

    public async Task<IActionResult> CategoryHierarchy()
    {
        var categories = await _context.TicketCategories
            .Include(c => c.Tickets)
                .ThenInclude(t => t.Status)
            .ToListAsync();

        return View(categories);
    }
}