using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SA_Management.Models;
public class CompanyPortfolio
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)] // Specify that ID is provided manually
    public Guid CompanyPortfolioID { get; set; }
    [Required]
    [ForeignKey("ShareCompany")]
    public Guid CompanyID { get; set; } // Foreign Key to ShareCompany

    public int QuantityWithoutBroker { get; set; } // Quantity bought without a broker

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AveragePriceWithoutBroker { get; set; } // Average price without a broker
    public int FUTQuantityWithoutBroker { get; set; } // Quantity bought without a broker

    [Column(TypeName = "decimal(18, 2)")]
    public decimal FUTAveragePriceWithoutBroker { get; set; } // Average price without a broker
    public int QuantityWithBroker { get; set; } // Quantity bought with a broker

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AveragePriceWithBroker { get; set; } // Average price with a broker
    public int FUTQuantityWithBroker { get; set; } // Quantity bought with a broker

    [Column(TypeName = "decimal(18, 2)")]
    public decimal FUTAveragePriceWithBroker { get; set; } // Average price with a broker
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalInvestnment { get; set; } // Total Investnment

    public int SoldQuantity { get; set; } // Total quantity sold

    public bool IsActive { get; set; } = true;

    // Navigation property
    public ShareCompany? ShareCompany { get; set; } // Link to ShareCompany
}