using System.Data.SqlTypes;

namespace Exam.Repository.Interfaces.BankAccount
{
    public class BankAccountUpdate
    {
        public SqlDecimal? Balance { get; set; }

    }
}
