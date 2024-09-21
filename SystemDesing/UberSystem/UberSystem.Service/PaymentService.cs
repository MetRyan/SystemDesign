using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberSystem.Domain.Entities;
using UberSystem.Domain.Interfaces;
using UberSystem.Domain.Interfaces.Services;

namespace UberSystem.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddPayment(Payment payment)
        {
            try { 
                var paymentRepository = _unitOfWork.Repository<Payment>(); 
            await _unitOfWork.BeginTransaction();
                await paymentRepository.InsertAsync(payment);
                await _unitOfWork.CommitTransaction();
            }catch { 
            await _unitOfWork.RollbackTransaction();
                throw;

            }
        }
    }
}
