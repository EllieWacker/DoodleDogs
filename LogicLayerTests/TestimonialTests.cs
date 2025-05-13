using DataAccessFakes;
using DataDomain;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayerTests
{
    [TestClass]
    public class TestimonialManagerTests
    {
        private ITestimonialManager? _testimonialManager;


        [TestInitialize]
        public void TestSetup()
        {
            _testimonialManager = new TestimonialManager(new TestimonialAccessorFake()); 

        }

        [TestMethod]
        public void TestRetrieveTestimonialByTestimonialIDReturnsCorrectTestimonial()
        {


            // arrange 
            const string expectedImage = "testTwizzler.jpg";
            const string testimonialID = "Livie family";
            string actualImage = "testTwizzler.jpg";

            // act
            actualImage = _testimonialManager.SelectTestimonialByTestimonialID(testimonialID).Image;

            // assert
            Assert.AreEqual(expectedImage, actualImage);
        }

        [TestMethod]
        public void TestRetrieveTestimonialsReturnsCorrectNumber()
        {

            // Arrange
            const int expectedNumber = 2;

            // Act
            List<Testimonial> testimonials = _testimonialManager.GetAllTestimonials();
            int actualNumber = testimonials.Count;

            // Assert
            Assert.AreEqual(expectedNumber, actualNumber);
        }
    }
}