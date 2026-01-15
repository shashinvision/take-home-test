using Fundo.Applications.WebApi.Models;
using Fundo.Applications.WebApi.Infraestructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fundo.Applications.WebApi.DTOs;
using System;

namespace Fundo.Applications.WebApi.Services;

public class PaymentManagementService : IPaymentManagementService
{
    private readonly IBaseRepository<Payment> _paymentRepository;
    private readonly IBaseRepository<Loan> _loanRepository;

    public PaymentManagementService(IBaseRepository<Payment> paymentRepository, IBaseRepository<Loan> loanRepository)
    {
        _paymentRepository = paymentRepository;
        _loanRepository = loanRepository;
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
        var loan = await _loanRepository.GetById(paymentDto.IdLoan);
        if (loan == null)
            throw new Exception("Loan not found");

        if (loan.CurrentBalance == 0)
            throw new Exception("Loan already paid");

        if (paymentDto.Amount > loan.CurrentBalance)
            throw new Exception("Payment amount exceeds loan balance");

        var payment = new Payment
        {
            Amount = paymentDto.Amount,
            IdLoan = paymentDto.IdLoan,
            CreatedAt = DateTime.Now,
        };

        await _paymentRepository.Add(payment);

        // update current balance of loan
        loan.CurrentBalance -= paymentDto.Amount;

        if (loan.CurrentBalance == 0)
        {
            loan.IsActive = 0;
        }
        await _loanRepository.Update(loan);
    }

    public async Task UpdatePayment(Payment payment)
    {
        await _paymentRepository.Update(payment);
    }

    public async Task DeletePayment(int id)
    {
        await _paymentRepository.Delete(id);
    }
}
