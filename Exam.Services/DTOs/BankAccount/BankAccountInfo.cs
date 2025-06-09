namespace Exam.Services.DTOs.BankAccount
{
    public class BankAccountInfo
    {
        public int BankAccountId { get; set; }
        public string IBAN { get; set; } 
        public decimal Balance { get; set; }
    }
}
