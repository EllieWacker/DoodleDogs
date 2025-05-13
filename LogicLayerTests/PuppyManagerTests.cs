using DataAccessFakes;
using DataDomain;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LogicLayerTests
{
    [TestClass]
    public class PuppyManagerTests
    {
        private IPuppyManager? _puppyManager;

        [TestInitialize]
        public void TestSetup()
        {
            _puppyManager = new PuppyManager(new PuppyAccessorFake()); 

        }

        [TestMethod]
        public void TestDeletePuppyReturnsTrue()
        {
            //arrange
            const string puppyID = "ALit1One";
            const int expectedResult = 1;
            int actualResult = 0;

            //act
            actualResult = _puppyManager.DeletePuppyByPuppyID(puppyID);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestDeletePuppyReturnsFalse()
        {
            //arrange
            const string puppyID = "1";

            //act
            _puppyManager.DeletePuppyByPuppyID(puppyID);
        }


        [TestMethod]
        public void TestInsertPuppyReturnsCorrectPrice()
        {
            //arrange
            const string puppyID = "aussiedoodle1";
            const string breedID = "aussie";
            const string litterID = "aussielit2";
            const string medicalRecordID = "shots";
            const string image = "puppy.jpg";
            const string gender = "male";
            const bool microchip = true;
            const bool adopted = true;
            const decimal price = 11.00M;

            const int expectedPuppyPrice = 11;
            int actualPuppyPrice = 0;

            //act
            actualPuppyPrice = _puppyManager.InsertPuppy(puppyID, breedID, litterID, medicalRecordID, image, gender, adopted, microchip, price);

            //assert
            Assert.AreEqual(expectedPuppyPrice, actualPuppyPrice);
        }

        [TestMethod]
        public void TestRetrievePuppyByPuppyIDReturnsCorrectPuppy()
        {
            // arrange 
            const string expectedImage = "fonzi.jpg";
            const string puppyID = "ALit1One";
            string actualImage = "fonzi.jpg";

            // act
            actualImage = _puppyManager.GetPuppyByPuppyID(puppyID).Image;

            // assert
            Assert.AreEqual(expectedImage, actualImage);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestGetPuppyByInvalidIDThrows()
        {
            _puppyManager.GetPuppyByPuppyID("nonexistent");
        }


        [TestMethod]
        public void TestRetrievePuppiesByBreedIDReturnsCorrectBreed()
        {

            // arrange 
            const string expectedBreed = "Aussiedoodle";
            const string breedID = "Aussiedoodle";

            // act
            List<Puppy> puppies = _puppyManager.SelectPuppiesByLitterID(breedID);

            // assert
            foreach (var puppy in puppies)
            {
                Assert.AreEqual(expectedBreed, puppy.BreedID);
            }
        }
        [TestMethod]
        public void TestUpdateFullPuppyReturns1ForSuccess()
        {
            // Arrange
            const string puppyID = "ALit1One";
            const string breedID = "Aussiedoodle";
            const string litterID = "AussieLit1";
            const string medicalRecordID = "Luke1";
            const string image = "fonzi.jpg";
            const string gender = "Female";
            const bool adopted = false;
            const bool microchip = true;
            const decimal price = 800.00m;

            bool expectedResult = true;

            // Act
            bool actualResult = _puppyManager.UpdateEntirePuppy(puppyID, breedID, litterID, medicalRecordID, image, gender, adopted, microchip, price);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void TestUpdateFullPuppyThrowsExceptionWhenPuppyNotFound()
        {
            // Arrange
            const string puppyID = "ALit1Two";
            const string breedID = "Aussiedoodle";
            const string litterID = "AussieLit1";
            const string medicalRecordID = "Luke1";
            const string image = "fonzi.jpg";
            const string gender = "Female";
            const bool adopted = false;
            const bool microchip = true;
            const decimal price = 800.00m;

            bool expectedResult = true;
            // Act and Assert
            bool actualResult =  _puppyManager.UpdateEntirePuppy(puppyID, breedID, litterID, medicalRecordID, image, gender, adopted, microchip, price);

            Assert.AreEqual(expectedResult, actualResult);
        }


        [TestMethod]
        public void TestSelectPuppiesByLitterIDReturnsCorrectBreed()
        {

            // arrange 
            const string expectedBreed = "Aussiedoodle";
            const string litterID = "AussieLit1";

            // act
            List<Puppy> puppies = _puppyManager.SelectPuppiesByLitterID(litterID);

            // assert
            foreach (var puppy in puppies)
            {
                Assert.AreEqual(expectedBreed, puppy.BreedID);
            }
        }

        [TestMethod]
        public void TestUpdatePuppyStatusReturns1ForSuccess()
        {
            //arrange
            const string puppyID = "ALit1One";
            const bool oldAdopted = false;
            const bool newAdopted = true;
            int expectedResult = 1;
            int actualResult = 0;

            //act
            actualResult = _puppyManager.UpdatePuppy(puppyID, oldAdopted, newAdopted);

            //assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestGetAllPuppiesReturnsCorrectNumber()
        {

            // Arrange
            const int expectedNumber = 2;

            // Act
            List<Puppy> puppies = _puppyManager.GetAllPuppies();
            int actualNumber = puppies.Count;

            // Assert
            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void TestGetAllPuppiesReturnsNotEmpty()
        {
            var puppies = _puppyManager.GetAllPuppies();
            Assert.IsTrue(puppies.Count > 0);
        }

    }
}