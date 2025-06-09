using Exam.Services.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Services.Interfaces.Payment
{
    public interface IPaymentService
    {
        public Task<List<PaymentInfo>> GetAllPaymentsAsync(int senderId);
        public Task<PaymentInfo?> GetPaymentByIdAsync(int paymentId);
        public Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);
        public Task<UpdatePaymentResponse> SendPaymentAsync(UpdatePaymentRequest request);
        public Task<UpdatePaymentResponse> CancelPaymentAsync(UpdatePaymentRequest request);

    }
}
