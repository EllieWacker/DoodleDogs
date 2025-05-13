using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomain
{
    public class User
    {
        public int UserID { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public bool Active { get; set; }
    }

    public class UserVM : User
    {
        public List<String>? Roles { get; set; }
    }
}
