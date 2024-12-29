using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SA_Management.DataAccessLayer.Data;
using SA_Management.Models;
using SA_Management.ViewModel;
using SA_Management.Services;
using Microsoft.AspNetCore.Http.Features;

namespace SA_Management.Controllers;

public class TradeController : Controller
{
    private readonly ILogger<TradeController> _logger;
    private readonly AppDbContext _appDbContext;
    public TradeController(ILogger<TradeController> logger, AppDbContext appDbContext)
    {
        _logger = logger;
        _appDbContext = appDbContext;
    }

    public async Task<IActionResult> Index(int PageIndex=1, int PageSize=10)
    {
        var Trades = _appDbContext.Trades.Include(x=>x.ShareCompany);
        var data = await GetPagination(Trades,PageIndex,PageSize);
        
        return View(data);
    }

    public async Task<PageResult<Trade>> GetPagination(IQueryable<Trade> DataList ,int PageIndex,int PageSize)
    {
        int TotalAmount = await DataList.CountAsync();
        var ListOfTrades = await DataList.OrderByDescending(x=>x.TradeID).Skip((PageIndex -1 )*PageSize).Take(PageSize).ToListAsync();
        return new PageResult<Trade>
        {
            PageIndex = PageIndex,
            PageSize = PageSize,
            TotalRecords = TotalAmount,
            Data = ListOfTrades
        };
    }

    [HttpGet]
    public async Task<IActionResult> CreatePurchase()
    {
        var shareCompanies = await _appDbContext.ShareCompanies.ToListAsync();
        CreatePurchaseTradeViewModel createPurchaseTradeViewModel = new CreatePurchaseTradeViewModel{
            ShareCompanies = shareCompanies.Select(x=> new SelectListItem{
                Value = x.CompanyID.ToString(),
                Text = x.CompanyName // Assuming ShareCompany has a Name property
            })
        };
        return View(createPurchaseTradeViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePurchase(CreatePurchaseTradeViewModel model)
    {
        model.Trade.TradeID = Guid.NewGuid();
        model.Trade.TradeType = "buy";
        var CompanyPortfolio = await FindCompanyPortfolioByID(model.Trade.CompanyID);
        TradeDetails tradeDetails = new TradeDetails();
        tradeDetails.TradeDetailsID = Guid.NewGuid();
        tradeDetails.TradeID = model.Trade.TradeID;
        if(model.Trade.TradeNature == "fut")
        {
            if(model.Trade.IsBrokerEngaged)
            {
                tradeDetails.AvgPrice = CompanyPortfolio.FUTAveragePriceWithBroker;
                tradeDetails.Quantity = CompanyPortfolio.FUTQuantityWithBroker;
                tradeDetails.TotalAmount = tradeDetails.Quantity*tradeDetails.AvgPrice;
                CompanyPortfolio.FUTAveragePriceWithBroker = GetNewPrice(CompanyPortfolio.FUTQuantityWithBroker, model.Trade.Quantity, CompanyPortfolio.FUTAveragePriceWithBroker, model.Trade.TradePrice);
                CompanyPortfolio.FUTQuantityWithBroker += model.Trade.Quantity;
                CompanyPortfolio.TotalInvestnment = CompanyPortfolio.TotalInvestnment + (model.Trade.Quantity*model.Trade.TradePrice); 
            }
            else
            {
                tradeDetails.AvgPrice = CompanyPortfolio.FUTAveragePriceWithoutBroker;
                tradeDetails.Quantity = CompanyPortfolio.FUTQuantityWithoutBroker;
                tradeDetails.TotalAmount = tradeDetails.Quantity*tradeDetails.AvgPrice;
                CompanyPortfolio.FUTAveragePriceWithoutBroker = GetNewPrice(CompanyPortfolio.FUTQuantityWithoutBroker, model.Trade.Quantity, CompanyPortfolio.FUTAveragePriceWithoutBroker, model.Trade.TradePrice);
                CompanyPortfolio.FUTQuantityWithoutBroker += model.Trade.Quantity;
                CompanyPortfolio.TotalInvestnment = CompanyPortfolio.TotalInvestnment + (model.Trade.Quantity*model.Trade.TradePrice);
            }
        }
        else if(model.Trade.TradeNature == "reg")
        {
            if(model.Trade.IsBrokerEngaged)
            {
                tradeDetails.AvgPrice = CompanyPortfolio.AveragePriceWithBroker;
                tradeDetails.Quantity = CompanyPortfolio.QuantityWithBroker;
                tradeDetails.TotalAmount = tradeDetails.Quantity*tradeDetails.AvgPrice;
                CompanyPortfolio.AveragePriceWithBroker = GetNewPrice(CompanyPortfolio.QuantityWithBroker, model.Trade.Quantity, CompanyPortfolio.AveragePriceWithBroker, model.Trade.TradePrice);
                CompanyPortfolio.QuantityWithBroker += model.Trade.Quantity;
                CompanyPortfolio.TotalInvestnment = CompanyPortfolio.TotalInvestnment + (model.Trade.Quantity*model.Trade.TradePrice); 
            }
            else
            {
                tradeDetails.AvgPrice = CompanyPortfolio.AveragePriceWithoutBroker;
                tradeDetails.Quantity = CompanyPortfolio.QuantityWithoutBroker;
                tradeDetails.TotalAmount = tradeDetails.Quantity*tradeDetails.AvgPrice;
                CompanyPortfolio.AveragePriceWithoutBroker = GetNewPrice(CompanyPortfolio.QuantityWithoutBroker, model.Trade.Quantity, CompanyPortfolio.AveragePriceWithoutBroker, model.Trade.TradePrice);
                CompanyPortfolio.QuantityWithoutBroker += model.Trade.Quantity;
                CompanyPortfolio.TotalInvestnment = CompanyPortfolio.TotalInvestnment + (model.Trade.Quantity*model.Trade.TradePrice);
            }
        }
        await _appDbContext.TradeDetails.AddAsync(tradeDetails); 
        await _appDbContext.Trades.AddAsync(model.Trade);
        await _appDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }


    [HttpGet]
    public async Task<IActionResult> CreateSale()
    {
        var shareCompanies = await _appDbContext.ShareCompanies.ToListAsync();
        CreateSaleTradeViewModel createSaleTradeViewModel = new CreateSaleTradeViewModel{
            ShareCompanies = shareCompanies.Select(x=> new SelectListItem{
                Value = x.CompanyID.ToString(),
                Text = x.CompanyName // Assuming ShareCompany has a Name property
            })
        };
        return View(createSaleTradeViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSale(CreateSaleTradeViewModel model)
    {
        model.Trade.TradeID = Guid.NewGuid();
        model.Trade.TradeType = "sell";
        var companyPortfolio = await FindCompanyPortfolioByID(model.Trade.CompanyID);
        TradeDetails tradeDetails = new TradeDetails();
        tradeDetails.TradeDetailsID = Guid.NewGuid();
        tradeDetails.TradeID = model.Trade.TradeID;
        if(model.Trade.TradeNature == "fut")
        {
            if(model.Trade.IsBrokerEngaged)
            {
                var broker = await _appDbContext.BrokerDetails.FirstOrDefaultAsync();
                broker.TotalBalance += ((model.Trade.TradePrice - companyPortfolio.FUTAveragePriceWithBroker) * model.Trade.Quantity) * (broker.FixedPriceCut/100);
                tradeDetails.AvgPrice = companyPortfolio.FUTAveragePriceWithBroker;
                tradeDetails.Quantity = companyPortfolio.FUTQuantityWithBroker;
                tradeDetails.TotalAmount = tradeDetails.Quantity*tradeDetails.AvgPrice;
                companyPortfolio.FUTQuantityWithBroker -= model.Trade.Quantity;
                companyPortfolio.SoldQuantity += model.Trade.Quantity;
                companyPortfolio.TotalInvestnment -= model.Trade.Quantity*companyPortfolio.FUTAveragePriceWithBroker;
                
                //update Broker's amount
            }
            else
            {
                tradeDetails.AvgPrice = companyPortfolio.FUTAveragePriceWithoutBroker;
                tradeDetails.Quantity = companyPortfolio.FUTQuantityWithoutBroker;
                tradeDetails.TotalAmount = tradeDetails.Quantity*tradeDetails.AvgPrice;
                companyPortfolio.FUTQuantityWithoutBroker -= model.Trade.Quantity;
                companyPortfolio.SoldQuantity += model.Trade.Quantity;
                companyPortfolio.TotalInvestnment -= model.Trade.Quantity*companyPortfolio.FUTAveragePriceWithoutBroker;
            }
        }
        else if(model.Trade.TradeNature == "reg")
        {
            if(model.Trade.IsBrokerEngaged)
            {
                var broker = await _appDbContext.BrokerDetails.FirstOrDefaultAsync();
                broker.TotalBalance += ((model.Trade.TradePrice - companyPortfolio.AveragePriceWithBroker) * model.Trade.Quantity) * (broker.FixedPriceCut/100);
                tradeDetails.AvgPrice = companyPortfolio.AveragePriceWithBroker;
                tradeDetails.Quantity = companyPortfolio.QuantityWithBroker;
                tradeDetails.TotalAmount = tradeDetails.Quantity*tradeDetails.AvgPrice;
                companyPortfolio.QuantityWithBroker -= model.Trade.Quantity;
                companyPortfolio.SoldQuantity += model.Trade.Quantity;
                companyPortfolio.TotalInvestnment -= model.Trade.Quantity*companyPortfolio.AveragePriceWithBroker;
                //update Broker's amount
            }
            else
            {
                tradeDetails.AvgPrice = companyPortfolio.AveragePriceWithoutBroker;
                tradeDetails.Quantity = companyPortfolio.QuantityWithoutBroker;
                tradeDetails.TotalAmount = tradeDetails.Quantity*tradeDetails.AvgPrice;
                companyPortfolio.QuantityWithoutBroker -= model.Trade.Quantity;
                companyPortfolio.SoldQuantity += model.Trade.Quantity;
                companyPortfolio.TotalInvestnment -= model.Trade.Quantity*companyPortfolio.AveragePriceWithoutBroker;
            }
        }
        await _appDbContext.TradeDetails.AddAsync(tradeDetails); 
        await _appDbContext.Trades.AddAsync(model.Trade);
        await _appDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet("api/portfolio/{companyId}")]
   public IActionResult GetPortfolio(Guid companyId, string nature, bool isBrokerEngaged = false)
    {
        // Validate the nature of shares (REG or FUT)
        if (string.IsNullOrEmpty(nature) || !(nature == "reg" || nature == "fut"))
        {
            return BadRequest("Invalid nature of shares. Must be 'REG' or 'FUT'.");
        }
        if(nature == "fut")
        {
            if(isBrokerEngaged)
            {
                var portfolio = _appDbContext.CompanyPortfolios
                .Where(p => p.CompanyID == companyId)
                .Select(p => new {
                    availableShares = p.FUTQuantityWithBroker,
                    averagePrice = p.FUTAveragePriceWithBroker
                }).FirstOrDefault();
                return Ok(portfolio);
            }
            else
            {
                var portfolio = _appDbContext.CompanyPortfolios
                .Where(p => p.CompanyID == companyId)
                .Select(p => new {
                    availableShares = p.FUTQuantityWithoutBroker,
                    averagePrice = p.FUTAveragePriceWithoutBroker
                }).FirstOrDefault();
                return Ok(portfolio);
            }
        }
        else if(nature == "reg")
        {
            if(isBrokerEngaged)
            {
                var portfolio = _appDbContext.CompanyPortfolios
                .Where(p => p.CompanyID == companyId)
                .Select(p => new {
                    availableShares = p.QuantityWithBroker,
                    averagePrice = p.AveragePriceWithBroker
                }).FirstOrDefault();
                return Ok(portfolio);
            }
            else
            {
                var portfolio = _appDbContext.CompanyPortfolios
                .Where(p => p.CompanyID == companyId)
                .Select(p => new {
                    availableShares = p.QuantityWithoutBroker,
                    averagePrice = p.AveragePriceWithoutBroker
                }).FirstOrDefault();
                return Ok(portfolio);
            }
        }        
        // // Simulate fetching portfolio data from the database
        // if (portfolio != null)
        // {
        //     return Ok(portfolio); // Return portfolio data in JSON format
        // }
        return NotFound(); // Return 404 if portfolio is not found
    }
    [HttpGet("Delete/{TradeId}")]
    public async Task<IActionResult> Delete(Guid TradeId)
    {
        var Trade = await _appDbContext.Trades.Where(x=>x.TradeID == TradeId).FirstOrDefaultAsync();
        Trade.ShareCompany = await _appDbContext.ShareCompanies.Where(x=>x.CompanyID == Trade.CompanyID).FirstOrDefaultAsync();
        return View(Trade);
    }
    [HttpPost("Delete/{id?}")]
    public async Task<IActionResult> Delete(Trade trade)
    {
        var TradeDetail = await _appDbContext.TradeDetails.Where(x=>x.TradeID == trade.TradeID).FirstOrDefaultAsync();
        var Trade = await _appDbContext.Trades.Where(x=>x.TradeID == trade.TradeID).FirstOrDefaultAsync();
        CompanyPortfolio? obj = await FindCompanyPortfolioByID(Trade.CompanyID);
        if(Trade.TradeType == "buy")
        {
            obj.TotalInvestnment -= Trade.Quantity*Trade.TradePrice;
            if(Trade.TradeNature == "fut")
            {
                if(Trade.IsBrokerEngaged)
                {
                    obj.FUTAveragePriceWithBroker = TradeDetail.AvgPrice;
                    obj.FUTQuantityWithBroker = TradeDetail.Quantity;
                }
                else
                {
                    obj.FUTAveragePriceWithoutBroker = TradeDetail.AvgPrice;
                    obj.FUTQuantityWithoutBroker = TradeDetail.Quantity;
                }
            }
            else if(Trade.TradeNature == "reg")
            {
                if(Trade.IsBrokerEngaged)
                {
                    obj.AveragePriceWithBroker = TradeDetail.AvgPrice;
                    obj.QuantityWithBroker = TradeDetail.Quantity;
                }
                else
                {
                    obj.AveragePriceWithoutBroker = TradeDetail.AvgPrice;
                    obj.QuantityWithoutBroker = TradeDetail.Quantity;
                }
            }
        }
        else if(Trade.TradeType == "sell")
        {
            obj.TotalInvestnment += TradeDetail.TotalAmount;    
            if(Trade.TradeNature == "fut")
            {
                if(Trade.IsBrokerEngaged)
                {
                    var broker = await _appDbContext.BrokerDetails.FirstOrDefaultAsync();
                    broker.TotalBalance -= ((Trade.TradePrice - TradeDetail.AvgPrice) * Trade.Quantity) * (broker.FixedPriceCut/100);
                    obj.FUTAveragePriceWithBroker = TradeDetail.AvgPrice;
                    obj.FUTQuantityWithBroker = TradeDetail.Quantity;
                }
                else
                {
                    obj.FUTAveragePriceWithoutBroker = TradeDetail.AvgPrice;
                    obj.FUTQuantityWithoutBroker = TradeDetail.Quantity;
                }
            }
            else if(Trade.TradeNature == "reg")
            {
                if(Trade.IsBrokerEngaged)
                {
                    var broker = await _appDbContext.BrokerDetails.FirstOrDefaultAsync();
                    broker.TotalBalance -= ((Trade.TradePrice - TradeDetail.AvgPrice) * Trade.Quantity) * (broker.FixedPriceCut/100);
                    obj.AveragePriceWithBroker = TradeDetail.AvgPrice;
                    obj.QuantityWithBroker = TradeDetail.Quantity;
                }
                else
                {
                    obj.AveragePriceWithoutBroker = TradeDetail.AvgPrice;
                    obj.QuantityWithoutBroker = TradeDetail.Quantity;
                }
            }
        }
        _appDbContext.TradeDetails.Remove(TradeDetail);
        _appDbContext.Trades.Remove(Trade);
        await _appDbContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    private async Task<CompanyPortfolio?> FindCompanyPortfolioByID(Guid companyGuid)
    {
        var comp = await _appDbContext.CompanyPortfolios.Where(x=>x.CompanyID == companyGuid).FirstOrDefaultAsync();
        return comp;
    }

    private Decimal GetNewPrice(int quantityP, int quantityN, decimal priceP, decimal priceN)
    {
        Decimal NewPrice = (priceP*quantityP) + (priceN*quantityN);
        NewPrice = NewPrice/(quantityP+quantityN);
        return NewPrice;
    }
    [HttpPost]
    public async Task<IActionResult> Delete()
    {
        var brokers = await _appDbContext.TradeDetails.ToListAsync();
        var trades = await _appDbContext.Trades.ToListAsync();
        _appDbContext.TradeDetails.RemoveRange(brokers);
        _appDbContext.Trades.RemoveRange(trades);
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update()
    {
        var Trades = await _appDbContext.Trades.ToListAsync();
        foreach (var item in Trades)
        {
            item.ShareCompany = await _appDbContext.ShareCompanies.Where(x=>x.CompanyID == item.CompanyID).FirstOrDefaultAsync();
        }
        InvoiceRenderingService inv = new InvoiceRenderingService();
        var document = inv.GenerateInvoicePdf(Trades);
        return File(document,"application/pdf","Invoice.pdf");
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
    //Reporting//
    
    [HttpGet]
    public async Task<IActionResult> DailyReporting()
    {
        var trades = _appDbContext.Trades.Where(x=>x.TradeDate.Year == 2024 && x.TradeDate.Month == 10 && x.TradeDate.Day == 31 && x.TradeType == "buy").ToList();
        foreach (var item in trades)
        {
            item.ShareCompany = await _appDbContext.ShareCompanies.Where(x=>x.CompanyID == item.CompanyID).FirstOrDefaultAsync();
        }
        return View(trades);
    }
    
    [HttpPost]
    public async Task<IActionResult> DailyReporting(DateOnly date, string tradeType, string tradeNature, 
    string companyId, bool isBrokerEngaged)
    {
        var trades = _appDbContext.Trades.Where(x=>x.TradeDate.Year == date.Year && x.TradeDate.Month == date.Month && x.TradeDate.Day == date.Day && x.TradeType == "buy").ToList();
        foreach (var item in trades)
        {
            item.ShareCompany = await _appDbContext.ShareCompanies.Where(x=>x.CompanyID == item.CompanyID).FirstOrDefaultAsync();
        }
        return View(trades);
    }
    
    [HttpGet("Trade/api/fetchCompanies")]
    public async Task<IActionResult> GetShareCompanies()
    {
        var companies = await _appDbContext.ShareCompanies.ToListAsync();
        return Ok(companies);
    }


}  