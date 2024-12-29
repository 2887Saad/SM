using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SA_Management.Models;
public class Trade
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)] // Specify that ID is provided manually
    public Guid TradeID { get; set; } // Use Guid instead of int

    [Required]
    [ForeignKey("ShareCompany")]
    public Guid CompanyID { get; set; } // ShareCompany should also use GUID

    [Required]
    public DateTime TradeDate { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Trade price must be greater than zero.")]
    public decimal TradePrice { get; set; }

    [Required]
    [MaxLength(4)] // 'Buy' or 'Sell'
    public string? TradeType { get; set; }

    [Required]
    [MaxLength(4)] // 'FUT' or 'REG'
    public string? TradeNature { get; set; } 

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; } 

    [NotMapped]
    public decimal Profit { get; set; }

    [Required]
    public bool IsBrokerEngaged { get; set; } // Whether a broker was used

    // Navigation properties
    public ShareCompany? ShareCompany { get; set; } // Link to ShareCompany
    public ICollection<TradeDetails>? TradeDetails { get; set; } // Link to TradeDetails


}