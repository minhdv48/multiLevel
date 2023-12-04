using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Multi_level.Models
{
    public class RegisModel
    {
        public string? FullName { get; set; }

        public string? Address { get; set; }
        [Required]
        public string? CodeRefer { get; set; }

        public string? Idcard { get; set; }

        public string? CardVerifyBy { get; set; }

        public DateTime? CardVerifyDate { get; set; }

        public string? PathInvoice { get; set; }

        public DateTime? DateJoin { get; set; }

        public string Email { get; set; }
        public string? Password { get; set; }
        [Required]
        public string Phone { get; set; }
        public string CaptchaCode { get; set; }
        [Required]
        public string AmwayCode { get; set; }
        public string? BankAccount { get; set; }
        public string? BankName { get; set; }
        public string? AccountName { get; set; }
        public string? Branch { get; set; }
    }

    public class InvoicesModel
    {
        public long Id { get; set; }
        public string? Path { get; set; }
        public int? ProfileId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateInvoice { get; set; }
        public string? InvoiceCode { get; set; }
        public List<IFormFile> Invoices { get; set; }
    }
    public class PaymentModel
    {
        public string? Path { get; set; }
        public int ProfileId { get; set; }
        public IFormFile img { get; set; }
    }


}
