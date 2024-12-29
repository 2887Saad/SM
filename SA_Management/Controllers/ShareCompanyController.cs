using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SA_Management.DataAccessLayer.Data;
using SA_Management.Models;

namespace SA_Management.Controllers;

public class ShareCompanyController : Controller
{
    private readonly ILogger<ShareCompanyController> _logger;
    private readonly AppDbContext _appDbContext;
    public ShareCompanyController(ILogger<ShareCompanyController> logger, AppDbContext appContext)
    {
        _appDbContext = appContext;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        List<ShareCompany> shareCompanies = await _appDbContext.ShareCompanies.ToListAsync();
        return View(shareCompanies);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ShareCompany shareCompany)
    {
        CompanyPortfolio  obj = new CompanyPortfolio();
        shareCompany.CompanyID = Guid.NewGuid();
        await _appDbContext.ShareCompanies.AddAsync(shareCompany);
        obj.CompanyPortfolioID = Guid.NewGuid();
        obj.CompanyID = shareCompany.CompanyID;
        obj.AveragePriceWithBroker = 0;
        obj.AveragePriceWithoutBroker = 0;
        obj.QuantityWithBroker = 0;
        obj.QuantityWithoutBroker = 0;
        obj.TotalInvestnment = 0;
        obj.SoldQuantity = 0;
        await _appDbContext.CompanyPortfolios.AddAsync(obj); 
        await _appDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet("[controller]/Update/{shareCompanyId?}")]
    public async Task<IActionResult> Update(Guid shareCompanyId)
    {
        Console.WriteLine(shareCompanyId);
        var shareCompany = await _appDbContext.ShareCompanies.Where(x=>x.CompanyID == shareCompanyId).FirstOrDefaultAsync();
        return View(shareCompany);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ShareCompany model)
    {
        var shareCompany = await _appDbContext.ShareCompanies.Where(x=>x.CompanyID == model.CompanyID).FirstOrDefaultAsync();
        shareCompany.CompanyName = model.CompanyName; 
        await _appDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete()
    {
        var brokers = await _appDbContext.ShareCompanies.ToListAsync();
        _appDbContext.ShareCompanies.RemoveRange(brokers);
        return RedirectToAction("Index");
    }
}