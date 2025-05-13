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
    public class FatherDogManagerTests
    {
        private IFatherDogManager? _fatherDogManager;
        private List<FatherDog> _fatherDogs;


        [TestInitialize]
        public void TestSetup()
        {
            _fatherDogManager = new FatherDogManager(new FatherDogAccessorFake()); 

        }

        [TestMethod]
        public void TestRetrieveFatherDogsReturnsCorrectNumber()
        {

            // Arrange
            const int expectedNumber = 2;

            // Act
            _fatherDogs = _fatherDogManager.GetAllFatherDogs();
            int actualNumber = _fatherDogs.Count;

            // Assert
            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void TestSelectFatherDogByFatherDogIDReturnsCorrectFatherDog()
        {


            // arrange 
            const string expectedImage = "cockapoo.jpg";
            const string fatherDogID = "Fonzi";
            string actualImage = "cockapoo.jpg";

            // act
            actualImage = _fatherDogManager.SelectFatherDogByFatherDogID(fatherDogID).Image;

            // assert
            Assert.AreEqual(expectedImage, actualImage);
        }
    }
}