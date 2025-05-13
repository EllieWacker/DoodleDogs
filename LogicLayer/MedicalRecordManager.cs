using DataAccessInterfaces;
using DataAccessLayer;
using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class MedicalRecordManager : IMedicalRecordManager
    {
        private IMedicalRecordAccessor _medicalrecordAccessor;

        public MedicalRecordManager()
        {
            _medicalrecordAccessor = new MedicalRecordAccessor();

        }

        public MedicalRecordManager(IMedicalRecordAccessor medicalrecordAccessor)
        {
            _medicalrecordAccessor = medicalrecordAccessor;

        }
        public MedicalRecord SelectMedicalRecordByMedicalRecordID(string medicalrecordID)
        {
            MedicalRecord medicalrecord = null;
            try
            {
                medicalrecord = _medicalrecordAccessor.SelectMedicalRecordByMedicalRecordID(medicalrecordID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("MedicalRecord not found", ex);
            }
            return medicalrecord;
        }

    }
}
