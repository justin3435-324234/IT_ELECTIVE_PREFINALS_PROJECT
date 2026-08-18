using IT_ELECTIVE_PREFINALS_PROJECT.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace IT_ELECTIVE_PREFINALS_PROJECT.Controllers;

public class ReportsController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(AppDbContext context, ILogger<ReportsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            ViewBag.TotalTickets = await _context.Tickets.CountAsync();
            ViewBag.UnassignedTicketsCount = await _context.Tickets
                .Where(t => !t.TicketAssignments.Any())
                .CountAsync();
            ViewBag.TotalEmployees = await _context.Employees.CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading Reports Index metrics");
            ViewBag.TotalTickets = 0;
            ViewBag.UnassignedTicketsCount = 0;
            ViewBag.TotalEmployees = 0;
        }

        return View();
    }

    public async Task<IActionResult> Workload()
    {
        var workload = await _context.Employees
            .AsNoTracking()
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
            .AsNoTracking()
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
            .AsNoTracking()
            .Include(c => c.Tickets)
                .ThenInclude(t => t.Status)
            .ToListAsync();

        return View(categories);
    }
}