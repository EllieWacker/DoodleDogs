using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomain
{
    public class Testimonial
    {
        public string? TestimonialID { get; set; }
        public int AdoptionID { get; set; }
        public string? Image { get; set; }
        public string? Details { get; set; }
        public int Rating { get; set; }
    }
}
