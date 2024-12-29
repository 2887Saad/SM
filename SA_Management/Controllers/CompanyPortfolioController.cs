using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SA_Management.DataAccessLayer.Data;
using SA_Management.Models;

namespace SA_Management.Controllers;

public class CompanyPortfolioController : Controller
{
    private readonly ILogger<CompanyPortfolioController> _logger;
    private readonly AppDbContext _appDbContext;
    public CompanyPortfolioController(ILogger<CompanyPortfolioController> logger, AppDbContext appContext)
    {
        _appDbContext = appContext;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        List<CompanyPortfolio> companyPortfolios = await _appDbContext.CompanyPortfolios.ToListAsync();
        foreach (var item in companyPortfolios)
        {
            item.ShareCompany = await _appDbContext.ShareCompanies.Where(x=>x.CompanyID == item.CompanyID).FirstOrDefaultAsync();
        }
        return View(companyPortfolios);
    }
    [HttpPost]
    public async Task<IActionResult> Delete()
    {
        var brokers = await _appDbContext.CompanyPortfolios.ToListAsync();
        _appDbContext.CompanyPortfolios.RemoveRange(brokers);
        return RedirectToAction("Index");
    }
}