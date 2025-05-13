using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomain
{
    public class Adoption
    {
        public int AdoptionID { get; set; }
        public int ApplicationID { get; set; }
        public string? PuppyID { get; set; }
        public int UserID { get; set; }
        public string? Status { get; set; }
    }
}
