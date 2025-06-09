using System.Data.SqlTypes;

namespace Exam.Repository.Interfaces.BankAccount
{
    public class BankAccountFilter
    {
        public SqlString? IBAN { get; set; }

        public SqlInt32? BankAccountId { get; set; }

    }
}
