using System.Data.SqlTypes;

namespace Exam.Repository.Interfaces.Payment
{
    public class PaymentFilter
    {

        public SqlInt32? StatusId { get; set; }
        public SqlInt32? SenderUserId { get; set; }

    }
}
