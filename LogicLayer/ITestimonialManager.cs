using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface ITestimonialManager
    {
        public Testimonial SelectTestimonialByTestimonialID(string testimonialID);
        public List<Testimonial> GetAllTestimonials();
    }
}
