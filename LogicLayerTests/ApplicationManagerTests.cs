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
    public class ApplicationManagerTests
    {
        private IApplicationManager? _applicationManager;


        [TestInitialize]
        public void TestSetup()
        {
            _applicationManager = new ApplicationManager(new ApplicationAccessorFake()); 

        }

        [TestMethod]
        public void TestRetrieveApplicationByUserIDReturnsCorrectApplication()
        {
            // arrange 
            const string expectedFullName =  "Clemmy";
            const int userID = 1;
            string actualFullName = "Clemmy";

            // act
            List<Application> applications = _applicationManager.SelectApplicationsByUserID(userID);
            foreach (Application app in applications) 
            {
                Assert.AreEqual(expectedFullName, app.FullName);
            }
        }

        [TestMethod]
        public void TestUpdateApplicationReturns1ForTrue()
        {
            //arrange
            const int applicationID = 1;
            const bool oldStatus = false;
            const bool newStatus = true;
            const int expectedResult = 1;

            //act
            int actualResult = _applicationManager.UpdateApplicationStatus(applicationID, oldStatus, newStatus);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestInsertApplicationReturnsCorrectID()
        {
            //arrange
            const int userID = 1;
            const string fullName = "Ellie";
            const int age = 19;
            const bool renting = false;
            const bool yard = true;
            const string desiredBreed = "Aussie";
            const string desiredGender = "Female";
            const string preferredContact = "Phone";
            const bool status = false;
            const string comment = "Hi";

            const int expectedApplicationID = 1; 
            int actualApplicationID = 0;

            //act
            actualApplicationID = _applicationManager.InsertApplication(userID, fullName, age, renting, yard, desiredBreed, desiredGender, preferredContact, status, comment);

            //assert
            Assert.AreEqual(expectedApplicationID, actualApplicationID);
        }

        [TestMethod]
        public void TestGetApplicationsReturnsList()
        {

            // arrange 
            const int expectedCount = 2;

            // act
            List<Application> applications = _applicationManager.GetAllApplications();

            // assert
            Assert.AreEqual(applications.Count(), expectedCount);
        }
    }
}