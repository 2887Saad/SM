using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SA_Management.Models;
public class BrokerDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)] // Specify that ID is provided manually
    public Guid BrokerID { get; set; } // Unique ID for the broker transaction

    [Required]
    public string? Name { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal FixedPriceCut { get; set; } // Fixed broker fee

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalBalance { get; set; } // Running total of broker earnings
    public bool IsActive { get; set; } = true;
}



//For PDF

// <button onclick="downloadPdf()">Download PDF</button>

// <script>
//     function downloadPdf() {
//         fetch('/DownloadPdf')
//             .then(response => {
//                 if (!response.ok) throw new Error('Network response was not ok');
//                 return response.blob();
//             })
//             .then(blob => {
//                 // Create a temporary URL for the blob object
//                 const url = window.URL.createObjectURL(blob);
//                 const a = document.createElement('a');
//                 a.href = url;
//                 a.download = "report.pdf";  // Set the file name
//                 document.body.appendChild(a);  // Append anchor to body
//                 a.click();  // Trigger download
//                 document.body.removeChild(a);  // Remove anchor after download
//                 window.URL.revokeObjectURL(url);  // Release blob URL
//             })
//             .catch(error => console.error('Error downloading PDF:', error));
//     }
// </script>