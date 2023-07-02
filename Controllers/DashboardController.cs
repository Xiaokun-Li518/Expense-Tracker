using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


namespace Expense_Tracker.Controllers;

public class DashboardController : Controller
{

    private readonly ApplicationDbContext _context;
    private readonly ILogger<DashboardController> _logger;
    public DashboardController(ApplicationDbContext context, ILogger<DashboardController> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<ActionResult> Index()
    {
        // Last 7 Days
        DateTime StartDate = DateTime.Today.AddDays (-6);
        DateTime EndDate = DateTime.Today;

        List<Transaction> SelectedTranscations = await _context.Transactions
            .Include(x => x.Category)
            .Where(y => y.Date >= StartDate && y.Date <= EndDate)
            .ToListAsync();

        // Total Income 
        int TotalIncome = (int)SelectedTranscations
            .Where (i => i.Category.Type == "Income")
            .Sum (j => j.Amount);

        ViewBag.TotalIncome = TotalIncome.ToString("c0");

        // Total Expense    
        int TotalExpense = (int)SelectedTranscations
            .Where (i => i.Category.Type == "Expense")
            .Sum (j => j.Amount);

        ViewBag.TotalExpense = TotalExpense.ToString("c0");

        // Balance 
        int Balance = TotalIncome - TotalExpense;
        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
        culture.NumberFormat.CurrencyNegativePattern = 1;
        ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);


        var doughnutChartData = SelectedTranscations
            .Where (i => i.Category.Type == "Expense")
            .GroupBy (j => j.CategoryId)
            .Select (k => new {
                CategoryTitleWithIcon = k.First().CategoryTitleWithIcon + " $" + k.Sum(j=>j.Amount),
                amount = k.Sum (j => j.Amount),
                formattedAmount = "$"+k.Sum(j=>j.Amount),
            })
            .OrderByDescending(l=>l.amount)
            .ToList();

        _logger.LogInformation("Doughnut Chart Data: {data}", doughnutChartData);
        ViewBag.DoughnutChartData = doughnutChartData;


        return View();
    }

}