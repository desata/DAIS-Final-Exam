namespace Exam.Services.DTOs.Payment
{
    public class CreatePaymentRequest
    {
        public int SenderBankAccountId { get; set; }
        public int SenderUserId { get; set; }
        public required string SenderIBAN { get; set; }
        public required string RecieverIBAN { get; set; }
        public required string RecieverName { get; set; }
        public required string Reference { get; set; }
        public decimal Amount { get; set; }
        public int StatusId { get; set; }
        
    }
}
