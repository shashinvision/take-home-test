using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Fundo.Applications.WebApi.Models
{
    public partial class Applicant
    {
        public Applicant()
        {
            Loans = new HashSet<Loan>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Dni { get; set; }

        [JsonIgnore]
        public virtual ICollection<Loan> Loans { get; set; }
    }
}
