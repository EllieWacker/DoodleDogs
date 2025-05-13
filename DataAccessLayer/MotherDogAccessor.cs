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
    public class MotherDogAccessor : IMotherDogAccessor
    {

        public List<MotherDog> SelectAllMotherDogs()
        {
            List<MotherDog> motherDogs = new List<MotherDog>();

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_all_mother_dogs", conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var motherDog = new MotherDog()
                        {
                            MotherDogID = reader.GetString(0),
                            BreedID = reader.GetString(1),
                            Personality = reader.GetString(2),
                            EnergyLevel = reader.GetString(3),
                            BarkingLevel = reader.GetString(4),
                            Trainability = reader.GetString(5),
                            Image = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Description = reader.GetString(7),
                        };
                        motherDogs.Add(motherDog);
                    }
                }
                else
                {
                    throw new ArgumentException("MotherDog List not found");
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

            return motherDogs;
        }
        public MotherDog SelectMotherDogByMotherDogID(string motherDogID)
        {
            MotherDog motherDog = null;

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_mother_dog_by_motherDogID", conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            //parameters
            cmd.Parameters.Add("@MotherDogID", SqlDbType.NVarChar, 50);

            //values
            cmd.Parameters["@MotherDogID"].Value = motherDogID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    motherDog = new MotherDog()
                    {
                        MotherDogID = reader.GetString(0),
                        BreedID = reader.GetString(1),
                        Personality = reader.GetString(2),
                        EnergyLevel = reader.GetString(3),
                        BarkingLevel = reader.GetString(4),
                        Trainability = reader.GetString(5),
                        Image = reader.IsDBNull(6) ? null : reader.GetString(6),
                        Description = reader.GetString(7),
                    };
                }
                else
                {
                    throw new ArgumentException("MotherDog record not found");
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

            return motherDog;
        }

    }



}
