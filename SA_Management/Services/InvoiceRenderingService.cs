using SA_Management.Models;
using QuestPDF.Fluent;
using System.Reflection;
using System.ComponentModel;
using QuestPDF.Helpers;
using QuestPDF.Companion;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;


namespace  SA_Management.Services;

public class InvoiceRenderingService
{
    public InvoiceRenderingService()
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
    }
    public byte[] GenerateInvoicePdf(List<Trade> trade)
    {
        var document = Document.Create(container=>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                page.Header()
                .Row(row => 
                {
                    row.RelativeItem()
                        .Column(column =>{
                            column.Item()
                                .Text("COMPANY NAME")
                                .FontFamily("Arial")
                                .FontSize(20)
                                .Bold();
                            column.Item()
                                .Text("Company Address")
                                .FontFamily("Arial");
                        });
                });
                page.Content();
                page.Footer();
            });

        });
        document.ShowInCompanion();
        return document.GeneratePdf();
    }

    //Removing extra spaces from a paragraph 
    string RemoveExtraSpaces(string paragraph) 
    {
        
        if(string.IsNullOrWhiteSpace(paragraph))
        {
            return string.Empty;
        }
        StringBuilder stringBuilder = new StringBuilder();
        bool isPreviousSpace = false;
        foreach(char chr in paragraph.Trim())
        {
            if (chr != ' ')
            {
                stringBuilder.Append(chr);
                isPreviousSpace = false;
            }
            else if(!isPreviousSpace)
            {
                stringBuilder.Append(chr);
                isPreviousSpace = true;
            }
        }
        return stringBuilder.ToString();
    }
    // With Recurssion
    string RemoveExtraSpacesWithRecurssion1(string paragraph) 
    {
        if (string.IsNullOrWhiteSpace(paragraph))
        {
            return string.Empty;            
        }
        paragraph.Trim();
        return RemoveExtraSpacesWithRecurssion2(false, 0, paragraph);
    }

    string RemoveExtraSpacesWithRecurssion2(bool isPreviousSpace , int index ,string paragraph) 
    {
        
        if(index >= paragraph.Length)
        {
            return string.Empty;
        }
        if (paragraph[index] != ' ')
        {
            if(!isPreviousSpace)
            {
                return " " + RemoveExtraSpacesWithRecurssion2(true, index+1,paragraph);
            }
            else
            {
                return RemoveExtraSpacesWithRecurssion2(true, index+1,paragraph);
            }
        }
        else
        {
            return paragraph[index] + RemoveExtraSpacesWithRecurssion2(false, index+1,paragraph);
        }
    }
}