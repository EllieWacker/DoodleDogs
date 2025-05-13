using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface ITestimonialAccessor
    {
        Testimonial SelectTestimonialByTestimonialID(string testimonialID);
        List<Testimonial> SelectAllTestimonials();
    }
}
