using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Fundo.Applications.WebApi.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public int? IdLoan { get; set; }
        public DateTime? CreatedAt { get; set; }

        [JsonIgnore]
        public virtual Loan IdLoanNavigation { get; set; }
    }
}
