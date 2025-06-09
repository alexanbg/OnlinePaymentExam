using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OnlinePayments.Repositories.Helpers;
using OnlinePayments.Repositories.Interfaces;
using OnlinePayments.Services.DTOs.Payments;
using OnlinePayments.Services.Helpers;
using OnlinePayments.Services.Interfaces.Payment;

namespace OnlinePayments.Services.Implementation.Payment
{
    public class PaymentService : IPaymentService
    {
        private IPaymentRepository paymentRepository;
        private IAccountRepository accountRepository;
        private IUserAndAccountRepository useAndAccountRepository;

        public PaymentService(IPaymentRepository paymentRepository, IAccountRepository accountRepository, IUserAndAccountRepository useAndAccountRepository)
        {
            this.paymentRepository = paymentRepository;
            this.accountRepository = accountRepository;
            this.useAndAccountRepository = useAndAccountRepository;
        }

        public async Task<CancelPaymentResponse> CancelPayment(CancelPaymentRequest request)
        {
            if(request.PaymentId == null)
            {
                return new CancelPaymentResponse
                {
                    Success = false,
                    Error = "PaymentId is required"
                };
            }
            var payment = await paymentRepository.Retrieve(request.PaymentId);
            if (payment.Status != 1)
            {
                return new CancelPaymentResponse
                {
                    Success = false,
                    Error = "Payment's status must be Изчаква"
                };
            }
            UpdateCommand command = new UpdateCommand();
            command.AddCondition("Status", 3);

            var result = paymentRepository.Update(request.PaymentId,command);
            return new CancelPaymentResponse
            {
                Success = true
            };
        }

        public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request)
        {
            if(request.SenderAccountId == null||
                request.ReceiverAccountId == null ||
                request.Sum == null ||
                request.Description == null)
            {
                return new CreatePaymentResponse()
                {
                    Success = false,
                    Error = "All values are required"
                };
            }

            if(request.ReceiverAccountId == 0)
            {
                return new CreatePaymentResponse()
                {
                    Success = false,
                    Error = "Receiver was not found"
                };
            }

            if (request.Sum <= 0)
            {
                return new CreatePaymentResponse()
                {
                    Success = false,
                    Error = "The sum must be above 0 лв."
                };
            }

            var payment = new Models.Payment
            {
                CreationDate = DateTime.Now,
                Description = request.Description,
                Sum = request.Sum,
                Status = 1,
                SendingAccountId = request.SenderAccountId,
                ReceivingAccountId = request.ReceiverAccountId
            };

            var createdPayment = await paymentRepository.Retrieve(await paymentRepository.Create(payment));

            return new CreatePaymentResponse()
            {
                Success = true,
                CreatedPayment = await MapToPaymentInfo(createdPayment)
            };
        }

        public async Task<PaymentInfo> MapToPaymentInfo(Models.Payment payment)
        {
            var sendingAccount = await accountRepository.Retrieve(payment.SendingAccountId);
            var receivingAccount = await accountRepository.Retrieve(payment.ReceivingAccountId);
            return new PaymentInfo()
            {
                PaymentId = payment.PaymentId,
                CreationDate = payment.CreationDate,
                ReceivingAccountId = payment.ReceivingAccountId,
                ReceivingAccountNumber = receivingAccount.AccountNumber,
                Description = payment.Description,
                SendingAccountId = payment.SendingAccountId,
                SendingAccountNumber = sendingAccount.AccountNumber,
                Status = payment.Status,
                StatusName = StatusHelper.ToStatusString(payment.Status),
                Sum = payment.Sum,
            };
        }

        public async Task<SendPaymentResponse> SendPayment(SendPaymentRequest request)
        {
            if (request.PaymentId == null)
            {
                return new SendPaymentResponse
                {
                    Success = false,
                    Error = "PaymentId is required!"
                };
            }

            var payment = await paymentRepository.Retrieve(request.PaymentId);
            if (payment.Status != 1)
            {
                return new SendPaymentResponse
                {
                    Success = false,
                    Error = "Payment's status must be Изчаква"
                };
            }

            var senderAccount = await accountRepository.Retrieve(payment.SendingAccountId);
            if (payment.Sum > senderAccount.AvailableSum)
            {
                return new SendPaymentResponse
                {
                    Success = false,
                    Error = "Тази сметка няма достатъчно пари за да изпрати плащането"
                };
            }

            var receiverAccount = await accountRepository.Retrieve(payment.ReceivingAccountId);

            UpdateCommand update = new UpdateCommand();
            update.AddCondition("AvailableSum",senderAccount.AvailableSum-payment.Sum);
            var result1 = await accountRepository.Update(senderAccount.AccountId, update);

            update = new UpdateCommand();
            update.AddCondition("AvailableSum", receiverAccount.AvailableSum + payment.Sum);
            var result2 = await accountRepository.Update(receiverAccount.AccountId, update);

            update = new UpdateCommand();
            update.AddCondition("Status", 2);
            var result3 = await paymentRepository.Update(payment.PaymentId, update);


            return new SendPaymentResponse
            {
                Success = true
            };
        }

        public async Task<ShowPaymentsResponse> ShowPayments(ShowPaymentsRequest request)
        {
            if(request.UserId == null)
            {
                return new ShowPaymentsResponse()
                {
                    Success = false,
                    Error = "User is required"
                };
            }
            Filter filter = new Filter();
            filter.AddCondition("UserId",request.UserId);
            var userAccounts = await useAndAccountRepository.RetrieveCollection(filter);
            var userPayments = new List<PaymentInfo>();
            
            foreach (var userAccount in userAccounts)
            {
                filter = new Filter();
                filter.AddCondition("SendingAccountId",userAccount.AccountId);
                var paymentsInAccount = await paymentRepository.RetrieveCollection(filter);
                foreach (var payment in paymentsInAccount)
                {
                    userPayments.Add(await MapToPaymentInfo(payment));
                }
            }

            if (request.SortByStatus)
            {
                return new ShowPaymentsResponse()
                {
                    Success = true,
                    Payments = userPayments.OrderBy(x => x.Status)
                };
            }
            return new ShowPaymentsResponse()
            {
                Success = true,
                Payments = userPayments.OrderByDescending(x=>x.CreationDate)
            };
        }
    }
}
