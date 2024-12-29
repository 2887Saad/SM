using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SA_Management.Models;
public class TradeDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)] // Specify that ID is provided manually
    public Guid TradeDetailsID { get; set; } // Unique ID for trade details

    [Required]
    [ForeignKey("Trade")]
    public Guid TradeID { get; set; } // Foreign Key to Trade

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal AvgPrice { get; set; } // AvgPrice befor trade

    [Required]
    public int Quantity { get; set; } // Quantity befor trade

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; } // total Amount befor trade


    // Navigation property
    public Trade? Trade { get; set; } // Link to Trade
}