using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multi_LevelModels.Models;

public partial class Profile
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string? Address { get; set; }

    public int? CityId { get; set; }

    public int? DistrictId { get; set; }

    public int? WardId { get; set; }

    public string CodeRefer { get; set; }

    public string Idcard { get; set; }

    public string CardVerifyBy { get; set; }

    public DateTime? CardVerifyDate { get; set; }

    public string? PathInvoice { get; set; }

    public DateTime? DateJoin { get; set; }

    public DateTime? DateVerify { get; set; }

    public DateTime? ModifiedDate { get; set; }
    public string? Email { get; set; }
    public string Phone { get; set; }
    public int Levels { get; set; }
    public int ParentId { get; set; }
    public string? ReferBy { get; set; }
    public int ReferId { get; set; }
    public string? AmwayCode { get; set; }
    public string? BankAccount { get; set; }
    public string? BankName { get; set; }
    public string? AccountName { get; set; }
    public string? Branch { get; set; }
    public bool IsPay { get; set; }
    [NotMapped]
    public decimal benefit { get; set; }

}
