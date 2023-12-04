using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multi_LevelModels.Models;

public partial class Invoice
{
    public long Id { get; set; }

    public string? Path { get; set; }

    public int? ProfileId { get; set; }

    public DateTime? DateCreated { get; set; }
    public DateTime? DateInvoice { get; set; }
    public string? InvoiceCode { get; set; }
    public bool IsPay { get; set; }

    [NotMapped]
    public string FullName { get; set; }
    [NotMapped]
    public string ReferBy { get; set; }
    [NotMapped]
    public int Levels { get; set; }
    
}
