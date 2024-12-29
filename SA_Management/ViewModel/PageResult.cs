using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SA_Management.ViewModel;
public class PageResult<T>
{
    public int PageSize { get; set; }=10;
    public int PageIndex { get; set; } =1;
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<T> Data { get; set; }

}