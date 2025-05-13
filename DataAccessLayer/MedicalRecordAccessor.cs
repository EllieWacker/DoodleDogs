using DataAccessInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Data;
using System.Data;
using DataDomain;

namespace DataAccessLayer
{
    public class MedicalRecordAccessor : IMedicalRecordAccessor
    {
        public MedicalRecord SelectMedicalRecordByMedicalRecordID(string medicalRecordID)
        {
            MedicalRecord medicalRecord = null;

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_medical_record_by_medicalRecordID", conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            //parameters
            cmd.Parameters.Add("@MedicalRecordID", SqlDbType.NVarChar, 50);

            //values
            cmd.Parameters["@MedicalRecordID"].Value = medicalRecordID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    medicalRecord = new MedicalRecord()
                    {
                        MedicalRecordID = reader.GetString(0),
                        Treatment = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Weight = reader.GetInt32(2),
                        Issues = reader.IsDBNull(3) ? null : reader.GetString(3)
                    };
                }
                else
                {
                    throw new ArgumentException("Father Dog record not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return medicalRecord;
        }

    }



}
