using Fundo.Applications.WebApi.Models;
using Fundo.Applications.WebApi.Infraestructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.DTOs;
using System;

namespace Fundo.Applications.WebApi.Services;

public class PaymentManagementService
{
    private readonly IBaseRepository<Payment> _paymentRepository;

    public PaymentManagementService(IBaseRepository<Payment> paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<IEnumerable<Payment>> GetAllPayments()
    {
        return await _paymentRepository.GetAll();
    }

    public async Task<Payment> GetPaymentById(int id)
    {
        return await _paymentRepository.GetById(id);
    }

    public async Task CreatePayment(PaymentDto paymentDto)
    {


        var payment = new Payment
        {
            Amount = paymentDto.Amount,
            IdLoan = paymentDto.IdLoan,
            CreatedAt = DateTime.Now,
        };

        await _paymentRepository.Add(payment);
    }

    public async Task UpdateLoan(Payment payment)
    {
        await _paymentRepository.Update(payment);
    }

    public async Task DeleteLoan(int id)
    {
        await _paymentRepository.Delete(id);
    }
}
