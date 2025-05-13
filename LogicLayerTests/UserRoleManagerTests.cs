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
    public class UserRoleManagerTests
    {
        private IUserRoleManager? _userRoleManager;


        [TestInitialize]
        public void TestSetup()
        {
            _userRoleManager = new UserRoleManager(new UserRoleAccessorFake());
        }

        [TestMethod]
        public void TestInsertUserRoleReturnsUserRoleID()
        {
            //arrange
            const string userRoleID = "Job";
            const int userID = 6;
            const string description = "Good";

            const string expectedUserRoleID = "Job"; 
            string actualUserRoleID = "Job";

            //act
            actualUserRoleID = _userRoleManager.InsertUserRole(userRoleID, userID, description);

            //assert
            Assert.AreEqual(expectedUserRoleID, actualUserRoleID);
        }

        [TestMethod]
        public void TestUpdateRoleReturns1ForTrue()
        {
            //arrange
            const int userID = 1;
            const string oldUserRoleID = "User";
            const string newUserRoleID = "Adopter";
            const int expectedResult = 1;

            //act
            int actualResult = _userRoleManager.UpdateUserRole(userID, oldUserRoleID, newUserRoleID);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}