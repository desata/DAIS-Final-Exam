using System.ComponentModel.DataAnnotations;

namespace Exam.Services.DTOs.Payments
{
    public  class PaymentsInfo
    {
        public int PaymentId { get; set; }

        public int SenderBankAccountId { get; set; }
        public string SenderBankAccountIBAN { get; set; }

        public int SenderUserId { get; set; }
        public string SenderName { get; set; }

        public required string RecieverIBAN { get; set; }
        public required string RecieverName { get; set; }

        public required string Reference { get; set; }

        public decimal Amount { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
