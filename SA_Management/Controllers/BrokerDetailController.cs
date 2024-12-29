using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SA_Management.DataAccessLayer.Data;
using SA_Management.Models;

namespace SA_Management.Controllers;

public class BrokerDetailController : Controller
{
    private readonly ILogger<BrokerDetailController> _logger;
    private readonly AppDbContext _appDbContext;
    public BrokerDetailController(ILogger<BrokerDetailController> logger, AppDbContext appContext)
    {
        _appDbContext = appContext;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<BrokerDetails> brokerDetails = await _appDbContext.BrokerDetails.ToListAsync();
        return View(brokerDetails);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(BrokerDetails model)
    {
        model.BrokerID = Guid.NewGuid();
        model.IsActive = true;
        model.TotalBalance = 0;
        await _appDbContext.BrokerDetails.AddAsync(model);
        await _appDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult Delete()
    {
        return View();
    }

    public void MakingTheQuery()
    {
        List<Trade> trades = _appDbContext.Trades.ToList();
        List<TradeDetails> tradeDetails = _appDbContext.TradeDetails.ToList();
        //Inner Join   
        var result1 = trades.Join(tradeDetails, tradeDetails=>tradeDetails.TradeID, trades=>trades.TradeID,(trades,tradeDetails)=>new Trade{
            TradeID = trades.TradeID,
            CompanyID = trades.CompanyID,
            TradeNature = trades.TradeNature,
            TradeDate = trades.TradeDate,
            TradePrice = trades.TradePrice,
            TradeType = trades.TradeType            
        }).ToList();

        var result2 = trades.GroupJoin(tradeDetails, tradeDetails=>tradeDetails.TradeID, trades=>trades.TradeID,(trades,tradeDetails)=>new Trade{
            TradeID = trades.TradeID,
            CompanyID = trades.CompanyID,
            TradeNature = trades.TradeNature,
            TradeDate = trades.TradeDate,
            TradePrice = trades.TradePrice,
            TradeType = trades.TradeType            
        }).ToList();

        var result3 = trades.GroupJoin(tradeDetails, tradeDetails=>tradeDetails.TradeID, trades=>trades.TradeID,(trades,tradeDetails)=>new {trades,tradeDetails})
        .SelectMany(
            x=>x.tradeDetails.DefaultIfEmpty(),
            (trades,tradeDetails)=>new {
            TradeID = trades.trades.TradeID,
            CompanyID = trades.trades.CompanyID,
            TradeNature = trades.trades.TradeNature,
            TradeDate = trades.trades.TradeDate,
            TradePrice = trades.trades.TradePrice,
            TradeType = trades.trades .TradeType
            }     
        );

        var result4 = trades.GroupJoin(tradeDetails, tradeDetails=>tradeDetails.TradeID, trades=>trades.TradeID,(trades,tradeDetails)=>new {trades,tradeDetails})
        .SelectMany(
            x=>x.tradeDetails.DefaultIfEmpty(),
            (trades,tradeDetails)=>new {
            TradeID = trades.trades.TradeID,
            CompanyID = trades.trades.CompanyID,
            TradeNature = trades.trades.TradeNature,
            TradeDate = trades.trades.TradeDate,
            TradePrice = trades.trades.TradePrice,
            TradeType = trades.trades .TradeType
            }     
        );

        var str1 = trades?.GroupBy(x=>x.TradeDate.Month).Select(x=>new{
         
        });
        var str2 = trades?.GroupBy(x=> new{ x.TradeType, x.TradeNature}).Select(x=>new{
            TradeType = x.Key.TradeType,
            TradeNature = x.Key.TradeNature
        });

        // var departmentStats = employeeData
        // .GroupBy(emp => emp.Department)
        // .Select(group => new
        // {
        //     DepartmentName = group.Key,
        //     NumberOfEmployees = group.Count(),
        //     TotalSalary = group.Sum(emp => emp.Salary)
        // });

        var sar =  result4.Union(result3);  


        var result = result1.Select(x=>new{
            x.CompanyID,
            x.TradeID,
            x.TradeNature,
            x.TradeDetails
        });

    }
}
// var brokers = await _appDbContext.BrokerDetails.ToListAsync();
// _appDbContext.BrokerDetails.RemoveRange(brokers);
// var tradeDetails = await _appDbContext.TradeDetails.ToListAsync();
// _appDbContext.TradeDetails.RemoveRange(tradeDetails);
// var trades = await _appDbContext.Trades.ToListAsync();
// _appDbContext.Trades.RemoveRange(trades);
// var companyPortfolios = await _appDbContext.CompanyPortfolios.ToListAsync();
// _appDbContext.CompanyPortfolios.RemoveRange(companyPortfolios);
// var shareCompanies = await _appDbContext.ShareCompanies.ToListAsync();
// _appDbContext.ShareCompanies.RemoveRange(shareCompanies);
// await _appDbContext.SaveChangesAsync();
// return RedirectToAction("Index");


public class Employee
{
    public int Id { get; set; }
    public String Name { get; set; }
    public int Department { get; set; }
    public int Salary { get; set; }

}

public class Department
{
    public int Id { get; set; }

    public string DepartmentName { get; set; }
}
public class LINQ
{
    public IList<Employee> CreateDataEmployees()
    {
        var employeeData = new List<Employee>
        {
            new Employee { Id = 1, Name = "Alice", Department = 1, Salary = 60000 },
            new Employee { Id = 2, Name = "Bob", Department = 2, Salary = 75000 },
            new Employee { Id = 3, Name = "Charlie", Department = 3, Salary = 62000 },
            new Employee { Id = 4, Name = "Diana", Department = 4, Salary = 80000 },
            new Employee { Id = 5, Name = "Eve", Department = 5, Salary = 70000 }
        };
        return employeeData;
    }

    public IList<Department> CreateDataDepartment()
    {
        var DeptData = new List<Department>
        {
            new Department { Id = 1, DepartmentName = "Dept1"},
            new Department { Id = 2, DepartmentName = "Dept2"},
            new Department { Id = 3, DepartmentName = "Dept3"},
            new Department { Id = 4, DepartmentName = "Dept4"},
            new Department { Id = 5, DepartmentName = "Dept5"}
        };
        return DeptData;
    }

    public void MakeLINQ_1(){
        var Employees = CreateDataEmployees();
        var departmentStats = Employees
            .GroupBy(emp => emp.Department)
            .Select(group => new
            {
                DepartmentName = group.Key,
                NumberOfEmployees = group.Count(),
                TotalSalary = group.Sum(emp => emp.Salary)
            });

        foreach (var stats in departmentStats)
        {
            Console.WriteLine($"Department: {stats.DepartmentName}, Employees: {stats.NumberOfEmployees}, Total Salary: {stats.TotalSalary}");
        }

    }

    public void MakeLINQ_2()
    {
        var Employees = CreateDataEmployees();
        var dept = CreateDataDepartment();
        var departmentStats = Employees
            .GroupBy(emp => emp.Department)
            .Select(group => new
            {
                DepartmentName = dept.Where(x => x.Id == group.Key).Select(x => x.DepartmentName).FirstOrDefault(),
                NumberOfEmployees = group.Count(),
                TotalSalary = group.Sum(emp => emp.Salary)
            });

        foreach (var stats in departmentStats)
        {
            Console.WriteLine($"Department: {stats.DepartmentName}, Employees: {stats.NumberOfEmployees}, Total Salary: {stats.TotalSalary}");
        }

    }

    public void MakeLINQ_3()
    {
        var Employees = CreateDataEmployees();
        var dept = CreateDataDepartment();
        var departmentStats = Employees
            .OrderByDescending(x=>x.Salary)
            .Take(3)
            .Average(x=>x.Salary);




            // .GroupBy(emp => emp.Department)
            // .Select(group => new
            // {
            //     DepartmentName = dept.Where(x => x.Id == group.Key).Select(x => x.DepartmentName).FirstOrDefault(),
            //     NumberOfEmployees = group.Count(),
            //     TotalSalary = group.Sum(emp => emp.Salary)
            // });

        // foreach (var stats in departmentStats)
        // {
        //     Console.WriteLine($"Department: {stats.DepartmentName}, Employees: {stats.NumberOfEmployees}, Total Salary: {stats.TotalSalary}");
        // }












        // --------------------Filter for Exception
        // public class GlobalExceptionFilter : IExceptionFilter
        // {
        //     public void OnException(ExceptionContext context)
        //     {
        //         context.Result = new JsonResult(new
        //         {
        //             error = context.Exception.Message,
        //             statusCode = 500
        //         });
        //         context.ExceptionHandled = true;
        //     }
        // }
    }
}