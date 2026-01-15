using System;
using System.Collections.Generic;

namespace Fundo.Applications.WebApi.Models
{
    public partial class Loan
    {
        public Loan()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public decimal? CurrentBalance { get; set; }
        public int? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? IdApplicant { get; set; }

        public virtual Applicant IdApplicantNavigation { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
