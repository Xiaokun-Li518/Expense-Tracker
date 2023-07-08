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

        // Get 7 days of transactions
        List<Transaction> SelectedTranscations = await GetSelectedTransactions(StartDate, EndDate);

        // Total Income, Total Expense, Balance
        PrepareTotal (SelectedTranscations);

        // Donut chart
        PrepareDoughnutChart(SelectedTranscations);

        // Spline Chart - Income vs Expense
        PrepareSplineChart(SelectedTranscations, StartDate);


        // Recent Transations 
        ViewBag.RecentTransations = await _context.Transactions
            .Include(i => i.Category)
            .OrderByDescending (j => j.Date)
            .Take(5)
            .ToListAsync();

        return View();
    }

    private void PrepareTotal (List<Transaction> transactions)
    {
        // Total Income 
        int TotalIncome = (int)transactions
            .Where (i => i.Category.Type == "Income")
            .Sum (j => j.Amount);

        ViewBag.TotalIncome = TotalIncome.ToString("c0");

        // Total Expense    
        int TotalExpense = (int)transactions
            .Where (i => i.Category.Type == "Expense")
            .Sum (j => j.Amount);

        ViewBag.TotalExpense = TotalExpense.ToString("c0");

        // Balance 
        int Balance = TotalIncome - TotalExpense;
        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
        culture.NumberFormat.CurrencyNegativePattern = 1;
        ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);
    }

    private async Task<List<Transaction>> GetSelectedTransactions(DateTime StartDate, DateTime EndDate)
    {
        return await _context.Transactions
            .Include(x => x.Category)
            .Where (y => y.Date >= StartDate && y.Date  <= EndDate)
            .ToListAsync();
    }

    private void PrepareDoughnutChart(List<Transaction> transactions)
    {
        var doughnutChartData = transactions
            .Where(i => i.Category.Type == "Expense")
            .GroupBy(j => j.CategoryId)
            .Select(k => new {
                CategoryTitleWithIcon = k.First().CategoryTitleWithIcon + " $" + k.Sum(j => j.Amount),
                amount = k.Sum(j => j.Amount),
                formattedAmount = "$" + k.Sum(j => j.Amount),
            })
            .OrderByDescending(l => l.amount)
            .ToList();

        ViewBag.DoughnutChartData = doughnutChartData;
    }

    private void PrepareSplineChart(List<Transaction> transactions, DateTime StartDate)
    {

        // Income
        List<SplineChartData> IncomeSummary = transactions
            .Where(i=>i.Category.Type=="Income")
            .GroupBy(j=>j.Date)
            .Select(k=>new SplineChartData()
            {
                day = k.First().Date.ToString("dd-MMM"),
                income = (int)k.Sum(l=>l.Amount)
            })
            .ToList();

        // Expense 
        List<SplineChartData> ExpenseSummary = transactions 
            .Where(i=>i.Category.Type=="Expense")
            .GroupBy(j=>j.Date)
            .Select(k=>new SplineChartData()
            {
                day = k.First().Date.ToString("dd-MMM"),
                expense=(int)k.Sum(l=>l.Amount)
            })
            .ToList();

        // Combine Income & Expense
        string[] Last7Days = Enumerable.Range(0, 7)
            .Select(i=>StartDate.AddDays(i).ToString("dd-MMM"))
            .ToArray();


        ViewBag.SplineChartData = from day in Last7Days 
                                  join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                  from income in dayIncomeJoined.DefaultIfEmpty()
                                  join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                  from expense in expenseJoined.DefaultIfEmpty()
                                  select new 
                                  {
                                    day = day,
                                    income = income == null ? 0 : income.income,
                                    expense = expense == null ? 0 : expense.expense
                                  };

    }
}


public class SplineChartData 
{
    public string day;
    public int income;
    public int expense;
}