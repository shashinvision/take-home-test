using System;
using System.Collections.Generic;

namespace Fundo.Applications.WebApi.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? IsActive { get; set; }
    }
}
