using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Fundo.Applications.WebApi.DTOs;

public class LoanDto
{
    public decimal? Amount { get; set; }
    public decimal? CurrentBalance { get; set; }
    public int? IsActive { get; set; }
    public int? IdApplicant { get; set; }
}
