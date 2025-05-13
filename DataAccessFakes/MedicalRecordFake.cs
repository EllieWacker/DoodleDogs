using DataAccessInterfaces;
using DataDomain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class MedicalRecordAccessorFake : IMedicalRecordAccessor
    {
        private List<MedicalRecord> _medicalrecords;
        public MedicalRecordAccessorFake()
        {
            _medicalrecords = new List<MedicalRecord>();
            _medicalrecords.Add(new MedicalRecord() { MedicalRecordID = "LukeWacker1", Treatment = "shots", Weight=4, Issues="none" });
            _medicalrecords.Add(new MedicalRecord() { MedicalRecordID = "LukeWacker2", Treatment = "shots", Weight=3, Issues="none"});
        }
        public MedicalRecord SelectMedicalRecordByMedicalRecordID(string medicalrecordID)
        {
            foreach (var medicalrecord in _medicalrecords)
            {
                if (medicalrecord.MedicalRecordID == medicalrecordID)
                {
                    return medicalrecord;
                }
            }
            throw new ArgumentException("MedicalRecord record not found");
        }


    }
}
