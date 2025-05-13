using DataAccessFakes;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayerTests
{
    [TestClass]
    public class BreedManagerTests
    {
        private IBreedManager? _breedManager;


        [TestInitialize]
        public void TestSetup()
        {
            _breedManager = new BreedManager(new BreedAccessorFake());

        }

        [TestMethod]
        public void TestRetrieveBreedByBreedIDReturnsCorrectBreed()
        {


            // arrange 
            const string expectedImage = "fonzi.jpg";
            const string breedID = "German Shepard";
            string actualImage = "fonzi.jpg";

            // act
            actualImage = _breedManager.SelectBreedByBreedID(breedID).Image;

            // assert
            Assert.AreEqual(expectedImage, actualImage);
        }
    }
}