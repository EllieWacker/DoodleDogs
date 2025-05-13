using DataAccessInterfaces;
using DataDomain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class TestimonialAccessorFake : ITestimonialAccessor
    {
        private List<Testimonial> _testimonials;
        public TestimonialAccessorFake()
        {
            _testimonials = new List<Testimonial>();
            _testimonials.Add(new Testimonial() { TestimonialID = "Livie family", AdoptionID= 1000000, Image="testTwizzler.jpg", Details="Twizzler is an amazing Aussiedoodle puppy! She is so smart and loves to learn new tricks and she is very sweet with our young children!", Rating = 5 });
            _testimonials.Add(new Testimonial() { TestimonialID = "Luke family", AdoptionID = 1000003, Image = "testTwizzler2.jpg", Details = "Twizzler is great!", Rating = 5 });
        }
        public Testimonial SelectTestimonialByTestimonialID(string testimonialID)
        {
            foreach (var testimonial in _testimonials)
            {
                if (testimonial.TestimonialID == testimonialID)
                {
                    return testimonial;
                }
            }
            throw new ArgumentException("Testimonial record not found");
        }

        public List<Testimonial> SelectAllTestimonials()
        {
            List<Testimonial> testimonials = new List<Testimonial>();
            foreach (var testimonial in _testimonials)
            {
                testimonials.Add(testimonial);
            }
            return testimonials;
        }


    }
}
