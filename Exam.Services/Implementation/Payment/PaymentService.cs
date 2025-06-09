using Exam.Repository.Interfaces.BankAccount;
using Exam.Repository.Interfaces.Payment;
using Exam.Repository.Interfaces.Status;
using Exam.Repository.Interfaces.User;
using Exam.Services.DTOs.BankAccount;
using Exam.Services.DTOs.Payment;
using Exam.Services.Interfaces.BankAccount;
using Exam.Services.Interfaces.Payment;

namespace Exam.Services.Implementation.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IBankAccountService _bankAccountService;


        public PaymentService(IPaymentRepository paymentRepository, IUserRepository userRepository, IBankAccountRepository bankAccountRepository, IStatusRepository statusRepository)
        {
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _bankAccountRepository = bankAccountRepository;
            _statusRepository = statusRepository;
        }


        public async Task<UpdatePaymentResponse> CancelPaymentAsync(UpdatePaymentRequest request)
        {
            try
            {
                var payment = await _paymentRepository.RetrieveAsync(request.PaymentId);

                if (payment == null)
                {
                    throw new ArgumentNullException(nameof(request), "Payment not found");
                }
                if (payment.StatusId != 1)
                {
                    throw new ArgumentNullException(nameof(request), "Payment cannot be cancelled");
                }

                if (request.StatusId != 3)
                {
                    throw new ArgumentNullException(nameof(request), "Request cannot be null");
                }

                var update = new PaymentUpdate
                {
                    StatusId = 3
                };

                var success = await _paymentRepository.UpdateAsync(request.PaymentId, update);

                return new UpdatePaymentResponse
                {
                    Success = success,
                    Message = "Payment cancelled successfully"
                };
            }
            catch (Exception ex)
            {
                return new UpdatePaymentResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }

        }

        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
        {
            var senderBankAccount = await _bankAccountRepository.RetrieveAsync(request.SenderBankAccountId);


            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "CreatePaymentRequest cannot be null");
            }
            if (string.IsNullOrWhiteSpace(request.RecieverIBAN))
            {
                throw new ArgumentException("Receiver IBAN is required", nameof(request.RecieverIBAN));
            }
            if (senderBankAccount.IBAN == request.RecieverIBAN)
            {
                throw new ArgumentException("Receiver IBAN cannot be equal to sender IBAN", nameof(request.RecieverIBAN));
            }
            if (string.IsNullOrWhiteSpace(request.RecieverName))
            {
                throw new ArgumentException("Receiver Name is required", nameof(request.RecieverName));
            }
            if (string.IsNullOrWhiteSpace(request.Reference))
            {
                throw new ArgumentException("Reference is required", nameof(request.Reference));
            }
            if (request.Amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(request.Amount), "Amount must be greater than zero");
            }
            if (senderBankAccount.Balance < request.Amount)
            {
                throw new InvalidOperationException("Insufficient funds in sender's bank account");
            }
            var payment = new Models.Payment
            {
                SenderBankAccountId = request.SenderBankAccountId,
                SenderUserId = request.SenderUserId,
                RecieverIBAN = request.RecieverIBAN,
                RecieverName = request.RecieverName,
                Reference = request.Reference,
                Amount = request.Amount,
                StatusId = 1
            };

            var isCreated = await _paymentRepository.CreateAsync(payment);
            if (isCreated < 1)
            {
                return new CreatePaymentResponse
                {
                    Success = false,
                    Message = "Payment was not created"
                };
            }
            else
            {
                senderBankAccount.Balance -= request.Amount;

                var update = new UpdateBalanceRequest
                {
                    BankAccountId = senderBankAccount.BankAccountId,
                    NewBalance = senderBankAccount.Balance
                    
                };

                return new CreatePaymentResponse
                {
                    Success = true,
                    Message = "Payment created successfully"
                };
            }
        }

        public async Task<List<PaymentInfo>> GetAllPaymentsAsync(int senderId)
        {
            var paymentInfoList = new List<PaymentInfo>();

            var payments = await _paymentRepository.RetrieveCollectionAsync(new PaymentFilter { SenderUserId = senderId }).ToListAsync();

            foreach (var payment in payments)
            {
                var paymentInfo = await MapPaymentsInfo(payment);
                paymentInfoList.Add(paymentInfo);
            }

            return paymentInfoList;

        }

        public async Task<PaymentInfo?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _paymentRepository.RetrieveAsync(paymentId);

            if (payment == null)
            {
                return null;
            }

            return await MapPaymentsInfo(payment);

        }

        public async Task<UpdatePaymentResponse> SendPaymentAsync(UpdatePaymentRequest request)
        {
            try
            {
                var payment = await _paymentRepository.RetrieveAsync(request.PaymentId);

                if (payment == null)
                {
                    throw new ArgumentNullException(nameof(request), "Payment not found");
                }
                if (payment.StatusId != 1)
                {
                    throw new ArgumentNullException(nameof(request), "Payment cannot be send");
                }

                if (request.StatusId != 2)
                {
                    throw new ArgumentNullException(nameof(request), "Request cannot be null");
                }

                var update = new PaymentUpdate
                {
                    StatusId = 2
                };

                var success = await _paymentRepository.UpdateAsync(request.PaymentId, update);

                return new UpdatePaymentResponse
                {
                    Success = success,
                    Message = "Payment sent successfully"
                };
            }
            catch (Exception ex)
            {
                return new UpdatePaymentResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }


        private async Task<PaymentInfo> MapPaymentsInfo(Models.Payment payment)
        {   
            var bankAccount = await _bankAccountRepository.RetrieveAsync(payment.SenderBankAccountId);
            var status = await _statusRepository.RetrieveAsync(payment.StatusId);
            var sender = await _userRepository.RetrieveAsync(payment.SenderUserId);

            return new PaymentInfo
            {
                PaymentId = payment.PaymentId,
                SenderBankAccountId = payment.SenderBankAccountId,
                SenderBankAccountIBAN = bankAccount.IBAN,
                SenderUserId = payment.SenderUserId,
                SenderName = sender.Name,
                RecieverIBAN = payment.RecieverIBAN,
                RecieverName = payment.RecieverName,
                Reference = payment.Reference,
                Amount = payment.Amount,
                StatusId = payment.StatusId,
                StatusDescription = status.Description
            };
        }
    }
}


