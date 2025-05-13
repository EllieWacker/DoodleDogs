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
    public class LitterManagerTests
    {
        private ILitterManager? _litterManager;


        [TestInitialize]
        public void TestSetup()
        {
            _litterManager = new LitterManager(new LitterAccessorFake()); // needs data fakes

        }

        [TestMethod]
        public void TestDeleteLitterReturnsTrue()
        {
            //arrange
            const string litterID = "AussieLit5";
            const int expectedResult = 1;
            int actualResult = 0;

            //act
            actualResult = _litterManager.DeleteLitterByLitterID(litterID);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestDeleteLitterReturnsFalseForInvalidLitterID()
        {
            //arrange
            const string litterID = "1";

            //act
            _litterManager.DeleteLitterByLitterID(litterID);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestDeleteLitterThrowsExceptionForNullLitterID()
        {
            // act
            _litterManager.DeleteLitterByLitterID(null);
        }


        [TestMethod]
        public void TestRetrieveLitterByLitterIDReturnsCorrectLitter()
        {


            // arrange 
            const string expectedImage = "lit5.jpg";
            const string litterID = "AussieLit5";
            string actualImage = "lit5.jpg";

            // act
            actualImage = _litterManager.SelectLitterByLitterID(litterID).Image;

            // assert
            Assert.AreEqual(expectedImage, actualImage);
        }

        [TestMethod]
        public void TestRetrieveLittersReturnsCorrectNumber()
        {

            // Arrange
            const int expectedNumber = 2;

            // Act
            List<Litter> litters = _litterManager.GetAllLitters();
            int actualNumber = litters.Count;

            // Assert
            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void TestGetAllLittersReturnsNonEmptyList()
        {
            // act
            var litters = _litterManager.GetAllLitters();

            // assert
            Assert.IsTrue(litters.Any(), "GetAllLitters should return a non-empty list.");
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRetrieveLitterThrowsExceptionWhenNotFound()
        {
            // arrange
            const string litterID = "NonExistentLitter";

            // act
            var result = _litterManager.SelectLitterByLitterID(litterID);

            // assert: expect exception to be thrown
        }


        [TestMethod]
        public void TestInsertLitterReturnsCorrectNumberPuppies()
        {
            //arrange
            const string litterID = "aussiedoodle1";
            const string fatherDogID = "Finn";
            const string motherDogID = "Clemmy";
            const string image = "litter.jpg";
            DateTime dateOfBirth = DateTime.Now;
            DateTime goHomeDate = DateTime.Now;
            const int numberPuppies = 2;

            const int expectedNumberPuppies = 2;
            int actualNumberPuppies = 0;

            //act
            actualNumberPuppies = _litterManager.InsertLitter(litterID, fatherDogID, motherDogID, image, dateOfBirth, goHomeDate, numberPuppies);

            //assert
            Assert.AreEqual(expectedNumberPuppies, actualNumberPuppies);
        }

        [TestMethod]
        public void TestUpdateFullLitterReturns1ForSuccess()
        {
            // arrange
            const string litterID = "AussieLit5";
            const string fatherDogID = "Harold";
            const string motherDogID = "Clemmy";
            const string image = "lit5.jpg";
            DateTime dateOfBirth = new DateTime(2004, 12, 5);
            DateTime goHomeDate = new DateTime(2005, 2, 5);
            const int numberPuppies = 2;

            bool expectedResult = true;

            // act
            bool actualResult = _litterManager.UpdateLitter(litterID, fatherDogID, motherDogID, image, dateOfBirth, goHomeDate, numberPuppies);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void TestUpdateFullLitterThrowsExceptionWhenLitterNotFound()
        {
            // arrange
            const string litterID = "aussiedoodle1";
            const string fatherDogID = "Finn";
            const string motherDogID = "Clemmy";
            const string image = "litter.jpg";
            DateTime dateOfBirth = DateTime.Now;
            DateTime goHomeDate = DateTime.Now;
            const int numberPuppies = 2;

            bool expectedResult = true;

            // act
            bool actualResult = _litterManager.UpdateLitter(litterID, fatherDogID, motherDogID, image, dateOfBirth, goHomeDate, numberPuppies);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}