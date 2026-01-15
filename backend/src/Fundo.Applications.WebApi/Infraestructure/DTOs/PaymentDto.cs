using System;
using System.Collections.Generic;

namespace Fundo.Applications.WebApi.DTOs;

public class PaymentDto
{
    public decimal? Amount { get; set; }
    public int IdLoan { get; set; }
}
