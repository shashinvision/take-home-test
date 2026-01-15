using Fundo.Applications.WebApi.Models;
using Fundo.Applications.WebApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;


public interface IPaymentManagementService
{
    Task<IEnumerable<Payment>> GetAllPayments();
    Task<Payment> GetPaymentById(int id);
    Task CreatePayment(PaymentDto paymentDto);
    Task UpdateLoan(Payment payment);
    Task DeleteLoan(int id);
}
