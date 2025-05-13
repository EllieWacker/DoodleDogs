using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomain
{
    public class MedicalRecord
    {
        public string? MedicalRecordID { get; set; }
        public string? Treatment { get; set; }
        public int Weight { get; set; }
        public string? Issues { get; set; }
    }
}
