using DataAccessInterfaces;
using DataAccessLayer;
using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class TestimonialManager : ITestimonialManager
    {
        private ITestimonialAccessor _testimonialAccessor;

        public TestimonialManager()
        {
            _testimonialAccessor = new TestimonialAccessor();

        }

        public TestimonialManager(ITestimonialAccessor testimonialAccessor)
        {
            _testimonialAccessor = testimonialAccessor;

        }
        public Testimonial SelectTestimonialByTestimonialID(string testimonialID)
        {
            Testimonial testimonial = null;
            try
            {
                testimonial = _testimonialAccessor.SelectTestimonialByTestimonialID(testimonialID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Testimonial not found", ex);
            }
            return testimonial;
        }

        public List<Testimonial> GetAllTestimonials()
        {
            List<Testimonial> testimonials = new List<Testimonial>();
            try
            {
                testimonials = _testimonialAccessor.SelectAllTestimonials();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Testimonial list not found", ex);
            }
            return testimonials;
        }

    }
}
