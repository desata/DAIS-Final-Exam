namespace Exam.Web.Models.ViewModels
{
    public class AllPaymentsViewModel
    {
        public int PaymentId { get; set; }
        public string SenderIBAN { get; set; }
        public string SenderName { get; set; }
        public string RecieverIBAN { get; set; }
        public string RecieverName { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public int StatusId { get; set; }
        public string StatusDescription { get; set; }
    }
}
