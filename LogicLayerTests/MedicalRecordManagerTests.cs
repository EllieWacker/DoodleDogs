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
    public class MedicalRecordManagerTests
    {
        private IMedicalRecordManager? _medicalRecordManager;


        [TestInitialize]
        public void TestSetup()
        {
            _medicalRecordManager = new MedicalRecordManager(new MedicalRecordAccessorFake());

        }

        [TestMethod]
        public void TestRetrieveMedicalRecordByMedicalRecordIDReturnsCorrectTreatment()
        {


            // arrange 
            const string expectedTreatment = "shots";
            const string medicalrecordID = "LukeWacker1";
            string actualTreatment = "shots";

            // act
            actualTreatment = _medicalRecordManager.SelectMedicalRecordByMedicalRecordID(medicalrecordID).Treatment;

            // assert
            Assert.AreEqual(expectedTreatment, actualTreatment);
        }
    }
}