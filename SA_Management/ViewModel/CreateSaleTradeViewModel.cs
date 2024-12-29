using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using SA_Management.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SA_Management.ViewModel;
public class CreateSaleTradeViewModel
{
    public Trade? Trade { get; set; }
    public int AvailableQuantity { get; set; }
    public Decimal AvgPrice { get; set; }


    public IEnumerable<SelectListItem>? ShareCompanies { get; set; }
}