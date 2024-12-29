using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SA_Management.DataAccessLayer.Data;
using SA_Management.Models;

namespace SA_Management.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _appDbContext;

    public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _logger = logger;
    }

    public IActionResult Index()
    {
        // throw new Exception();
        List<ShareCompany> shareCompanies = _appDbContext.ShareCompanies.ToList();
        return View(shareCompanies);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}
