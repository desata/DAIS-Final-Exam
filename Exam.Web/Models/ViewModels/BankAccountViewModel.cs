using System.ComponentModel.DataAnnotations;

namespace Exam.Web.Models.ViewModels
{
    public class BankAccountViewModel
    {
        public int BankAccountId { get; set; }

        public string IBAN { get; set; }

        public decimal Balance { get; set; }

    }
}
