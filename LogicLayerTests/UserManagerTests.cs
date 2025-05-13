using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer;
using DataAccessFakes;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using DataDomain;

namespace LogicLayerTests
{
    [TestClass]
    public class UserManagerTests
    {
        private IUserManager? _userManager;


        [TestInitialize]
        public void TestSetup()
        {
            _userManager = new UserManager(new UserAccessorFake()); 
        }

        [TestMethod]
        public void TestUpdatePasswordReturnsTrueForSuccess()
        {
            //arrange
            const string email = "test1@test.com";
            const string oldPassword = "password";
            const string newPassword = "newpassword";
            const bool expectedResult = true;
            bool actualResult = false;

            //act
           actualResult = _userManager.UpdatePassword(email, oldPassword, newPassword);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
            Assert.IsTrue(_userManager.AuthenticateUser(email, newPassword));
        }

        [TestMethod]
        public void TestInsertUserReturnsNewUserID()
        {
            //arrange
            const string givenName = "Ellie";
            const string familyName = "Wacker";
            const string phoneNumber = "1234567890";
            const string email = "ellie@example.com";
            const int expectedUserID = 1000002; 
            int actualUserID = 0;

            //act
            actualUserID = _userManager.InsertUser(givenName, familyName, phoneNumber, email);

            //assert
            Assert.AreEqual(expectedUserID, actualUserID);
        }


        [TestMethod]
        public void TestGetRolesForUserReturnsCorrectList()
        {
            //arrange
            const int userID = 1;
            const int expectedRoleCount = 2;
            int actualRoleCount = 0;

            //act
            actualRoleCount = _userManager.GetRolesForUser(userID).Count();

            //assert
            Assert.AreEqual(expectedRoleCount, actualRoleCount);
        }


        [TestMethod]
        public void TestRetrieveUserByEmailReturnsCorrectUser()
        {
            // arrange 
            const int expectedUserID = 1;
            const string email = "test1@test.com";
            int actualUserID = 0;

            // act
            actualUserID = _userManager.RetrieveUserByEmail(email).UserID;

            // assert
            Assert.AreEqual(expectedUserID, actualUserID);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRetrieveUserByEmailFailsWithBadEmail()
        {
            // arrange 
            const string email = "bad@email.com";

            // act
            _userManager.RetrieveUserByEmail(email);
        }

        [TestMethod]
        public void TestAuthenticateUserReturnsTrueForGoodEmailAndPassword()
        {
            //arrange 
            const string email = "test1@test.com";
            const string password = "password";
            const bool expectedResult = true;
            bool actualResult = false;

            //act
            actualResult = _userManager.AuthenticateUser(email, password);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestAuthenticateUserReturnsFalseForBadEmailAndPassword()
        {
            //arrange 
            const string email =" bad1@test.com";
            const string password = "password";
            const bool expectedResult = false;
            bool actualResult = true;

            //act
            actualResult = _userManager.AuthenticateUser(email, password);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestAuthenticateUserReturnsFalseForBadPassword()
        {
            //arrange 
            const string email = " bad1@test.com";
            const string password = "badpassword";
            const bool expectedResult = false;
            bool actualResult = true;

            //act
            actualResult = _userManager.AuthenticateUser(email, password);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestAuthenticateUserReturnsFalseForInactiveUser()
        {
            //arrange 
            const string email = "test3@test.com";
            const string password = "password";
            const bool expectedResult = false;
            bool actualResult = true;
            
            //act
            actualResult = _userManager.AuthenticateUser(email, password);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestHashSHA256PasswordReturnsCorrectResult()
        {
            // Arrange
            const string valueToHash = "newuser";
            const string expectedHash = "9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e";
            var actualHash = "";

            //act
            actualHash = _userManager.HashSHA256(valueToHash);

            //assert
            Assert.AreEqual(expectedHash, actualHash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetHashSHA256ThrowsAnArgumentExceptionForEmptyString()
        {
            // arrange
            const string valueToHash = "";

            //act
            _userManager.HashSHA256(valueToHash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetHashSHA256ThrowsAnArgumentExceptionForNull()
        {
            // arrange
            const string valueToHash = null;

            //act
            _userManager.HashSHA256(valueToHash);
        }

        [TestMethod]
        public void TestRetrieveUsersReturnsCorrectNumber()
        {

            // Arrange
            const int expectedNumber = 3;

            // Act
            List<User> users = _userManager.SelectAllUsers();
            int actualNumber = users.Count;

            // Assert
            Assert.AreEqual(expectedNumber, actualNumber);
        }
    }
}
