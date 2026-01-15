using System;
using System.Collections.Generic;

namespace Fundo.Applications.WebApi.DTOs;

public class LoanDto
{
    public int Id { get; set; }
    public decimal? Amount { get; set; }
    public decimal? CurrentBalance { get; set; }
    public int? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public int? IdApplicant { get; set; }
    public ApplicantDto IdApplicantNavigation { get; set; }
    public List<PaymentDto> Payments { get; set; }
}
