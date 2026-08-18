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
}