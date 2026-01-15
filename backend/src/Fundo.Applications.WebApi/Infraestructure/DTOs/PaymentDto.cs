using System;
using System.Collections.Generic;

namespace Fundo.Applications.WebApi.DTOs;

public class PaymentDto
{
    public int Id { get; set; }
    public decimal? Amount { get; set; }
    public int? IdLoan { get; set; }
    public DateTime? CreatedAt { get; set; }
}
