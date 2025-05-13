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
    public class AdoptionManagerTests
    {
        private IAdoptionManager? _adoptionManager;


        [TestInitialize]
        public void TestSetup()
        {
            _adoptionManager = new AdoptionManager(new AdoptionAccessorFake());
        }

        [TestMethod]
        public void TestInsertAdoptionReturnsCorrectID()
        {
            //arrange
            const int applicationID = 2;
            const string puppyID = "Aussie";
            const int userID = 1;
            const string status = "Good";

            const int expectedAdoptionID = 1; 
            int actualAdoptionID = 0;

            //act
            actualAdoptionID = _adoptionManager.InsertAdoption(applicationID, puppyID, userID, status);

            //assert
            Assert.AreEqual(expectedAdoptionID, actualAdoptionID);
        }

        [TestMethod]
        public void TestGetAdoptionsReturnsList()
        {

            // arrange 
            const int expectedCount = 2;

            // act
            List<Adoption> adoptions = _adoptionManager.GetAllAdoptions();

            // assert
            Assert.AreEqual(adoptions.Count(), expectedCount);
        }

        [TestMethod]
        public void TestUpdateAdoptionReturns1ForSuccess()
        {
            //arrange
            const int adoptionID = 1;
            const string oldStatus = "In Progress";
            const string newStatus = "Adopted";
            int expectedResult = 1;
            int actualResult = 0;

            //act
            actualResult = _adoptionManager.UpdateAdoption(adoptionID, oldStatus, newStatus);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}