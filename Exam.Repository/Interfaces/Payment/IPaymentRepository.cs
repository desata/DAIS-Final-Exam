using Exam.Repository.Base;

namespace Exam.Repository.Interfaces.Payment
{
    public interface IPaymentRepository : IBaseRepository<Models.Payment, PaymentFilter, PaymentUpdate>
    {
    }
}
