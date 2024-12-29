using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SA_Management.Models;
public class ShareCompany
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)] // Specify that ID is provided manually
    public Guid CompanyID { get; set; } // GUID for Company ID

    [Required]
    [StringLength(100)]
    public string? CompanyName { get; set; } // Name of the company

    // Navigation property for trades
    public ICollection<Trade>? Trades { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation property for portfolio
    public CompanyPortfolio? CompanyPortfolio { get; set; }
}