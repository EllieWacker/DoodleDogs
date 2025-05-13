using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IMedicalRecordManager
    {
        public MedicalRecord SelectMedicalRecordByMedicalRecordID(string medicalrecordID);
    }
}
