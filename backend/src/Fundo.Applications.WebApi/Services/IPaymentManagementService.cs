using Fundo.Applications.WebApi.Models;
using Fundo.Applications.WebApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundo.Applications.WebApi.Services;

public interface IPaymentManagementService
{
    Task<IEnumerable<Payment>> GetAllPayments();
    Task<Payment> GetPaymentById(int id);
    Task CreatePayment(PaymentDto paymentDto);
    Task UpdatePayment(Payment payment);
    Task DeletePayment(int id);
}
