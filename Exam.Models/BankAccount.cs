using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class BankAccount
    {

        public int BankAccountId { get; set; }

        [Required(ErrorMessage = "IBAN is required")]
        [StringLength(22, MinimumLength = 22, ErrorMessage = "IBAN must be exactly 22 characters long")]
        public string IBAN { get; set; }

        [Required(ErrorMessage = "Balance is required")]
        public decimal Balance { get; set; }

    }
}

