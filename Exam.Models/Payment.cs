using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Sender Bank Account is required")]
        public int SenderBankAccountId { get; set; }

        [Required(ErrorMessage = "Sender User ID is required")]
        public int SenderUserId { get; set; }

        [Required(ErrorMessage = "Receiver Bank Account is required")]
        [StringLength(22, MinimumLength = 22, ErrorMessage = "IBAN must be exactly 22 characters long")]
        [RegularExpression("^[a-zA-Z0-9]{22}$", ErrorMessage = "Only numbers and Latin letters are allowed.")]

        public required string RecieverIBAN { get; set; }

        [Required(ErrorMessage = "Receiver Name is required")]
        [StringLength(100, ErrorMessage = "Receiver Name cannot exceed 100 characters")]
        public required string RecieverName { get; set; }

        [Required(ErrorMessage = "Reference is required")]
        [StringLength(32, ErrorMessage = "Reference cannot exceed 32 characters")]
        public required string Reference { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 9999999999999999.99, ErrorMessage = "Amount must be between 0.01 and 9999999999999999.99")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public int StatusId { get; set; }

    }
}