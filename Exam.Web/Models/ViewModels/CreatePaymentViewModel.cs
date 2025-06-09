using System.ComponentModel.DataAnnotations;

namespace Exam.Web.Models.ViewModels
{
    public class CreatePaymentViewModel
    {
        public int SenderBankAccountId { get; set; }

        [Required]
        public int SenderUserId { get; set; }

        [Required]
        [Display(Name = "Sender Name")]
        public string SenderName { get; set; }

        [Required]
        [Display(Name = "Sender IBAN")]
        public string SelectedIBAN { get; set; } 
        public List<BankAccountOption> IBANOptions { get; set; } = new();

        [Required]
        [Display(Name = "Receiver IBAN")]
        public string RecieverIBAN { get; set; }

        [Required]
        [Display(Name = "Receiver Name")]
        public string RecieverName { get; set; }

        [Display(Name = "Reference")]
        public string Reference { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        public int StatusId { get; set; } = 1; 
    }

    public class BankAccountOption
    {
        public int BankAccountId { get; set; }
        public string IBAN { get; set; }
    }
}