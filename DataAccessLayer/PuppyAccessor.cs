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
    public class PuppyAccessor : IPuppyAccessor
    {
        public int DeletePuppyByPuppyID(string puppyID)
        {
            int result = 0;

            // connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_delete_puppy_by_puppyID", conn);
            // type
            cmd.CommandType = CommandType.StoredProcedure;
            // parameters
            cmd.Parameters.Add("@PuppyID", SqlDbType.NVarChar,50);
            // values
            cmd.Parameters["@PuppyID"].Value = puppyID;
            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public int UpdateFullPuppy(string puppyID,string breedID,string litterID,string medicalRecordID,string image,string gender,bool adopted,bool microchip,decimal price)
        {
            int result = 0;

            // Connection
            var conn = DBConnection.GetConnection();

            // Command
            var cmd = new SqlCommand("sp_update_puppy_by_puppyid", conn);  
            cmd.CommandType = CommandType.StoredProcedure;

            // Parameters
            cmd.Parameters.Add("@PuppyID", SqlDbType.NVarChar, 50).Value = puppyID;
            cmd.Parameters.Add("@BreedID", SqlDbType.NVarChar, 50).Value = breedID;
            cmd.Parameters.Add("@LitterID", SqlDbType.NVarChar, 50).Value = litterID;

            if (string.IsNullOrEmpty(medicalRecordID))
            {
                cmd.Parameters.Add("@MedicalRecordID", SqlDbType.NVarChar, 50).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@MedicalRecordID", SqlDbType.NVarChar, 50).Value = medicalRecordID;
            }

            cmd.Parameters.Add("@Image", SqlDbType.NVarChar, 30).Value = image;
            cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 30).Value = gender;
            cmd.Parameters.Add("@Adopted", SqlDbType.Bit).Value = adopted;
            cmd.Parameters.Add("@Microchip", SqlDbType.Bit).Value = microchip;

            cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = price;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;  
            }
            finally
            {
                conn.Close();
            }

            return result;  
        }



        public int InsertPuppy(string puppyID, string breedID, string litterID, string medicalRecordID, string image, string gender, bool adopted, bool microchip, decimal price)
        {
            int result = 0;
            //connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_insert_puppy", conn);
            //type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@PuppyID", SqlDbType.NVarChar, 50).Value = puppyID;
            cmd.Parameters.Add("@BreedID", SqlDbType.NVarChar, 50).Value = breedID;
            cmd.Parameters.Add("@LitterID", SqlDbType.NVarChar, 50).Value = litterID;
            cmd.Parameters.Add("@MedicalRecordID", SqlDbType.NVarChar, 50).Value = medicalRecordID;
            cmd.Parameters.Add("@Image", SqlDbType.NVarChar, 30).Value = image;
            cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 30).Value = gender;
            cmd.Parameters.Add("@Adopted", SqlDbType.Bit).Value = adopted;
            cmd.Parameters.Add("@Microchip", SqlDbType.Bit).Value = microchip;
            cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = price;

            try
            {
                conn.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during insert: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }



        public Puppy SelectPuppyByPuppyID(string puppyID)
        {
            Puppy puppy = null;

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_puppy_by_puppyID", conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            //parameters
            cmd.Parameters.Add("@PuppyID", SqlDbType.NVarChar, 50);

            //values
            cmd.Parameters["@PuppyID"].Value = puppyID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    puppy = new Puppy()
                    {
                        PuppyID = reader.GetString(0),
                        BreedID = reader.GetString(1),
                        LitterID = reader.GetString(2),
                        MedicalRecordID = reader.GetString(3),
                        Image = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Gender = reader.GetString(5),
                        Adopted = reader.GetBoolean(6),
                        Microchip = reader.GetBoolean(7),
                        Price = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                    };
                }
                else
                {
                    throw new ArgumentException("Puppy record not found");
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

            return puppy;
        }
        public List<Puppy> SelectPuppiesByLitterID(string litterID)
        {
            List<Puppy> puppies = new List<Puppy>();

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_puppies_by_litterID", conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            //parameters
            cmd.Parameters.Add("@LitterID", SqlDbType.NVarChar, 50);

            //values
            cmd.Parameters["@LitterID"].Value = litterID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var puppy = new Puppy()
                        {
                            PuppyID = reader.GetString(0),
                            BreedID = reader.GetString(1),
                            LitterID = reader.GetString(2),
                            MedicalRecordID = reader.GetString(3),
                            Image = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Gender = reader.GetString(5),
                            Adopted = reader.GetBoolean(6),
                            Microchip = reader.GetBoolean(7),
                            Price = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                        };
                        puppies.Add(puppy);

                    }
                    
                }
                else
                {
                    throw new ArgumentException("Puppy record not found");
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

            return puppies;
        }


        public List<Puppy> SelectPuppiesByBreedID(string breedID)
        {
            List<Puppy> puppies = new List<Puppy>();

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_puppies_by_breedID", conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            //parameters
            cmd.Parameters.Add("@BreedID", SqlDbType.NVarChar, 50);

            //values
            cmd.Parameters["@BreedID"].Value = breedID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var puppy = new Puppy()
                        {
                            PuppyID = reader.GetString(0),
                            BreedID = reader.GetString(1),
                            LitterID = reader.GetString(2),
                            MedicalRecordID = reader.GetString(3),
                            Image = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Gender = reader.GetString(5),
                            Adopted = reader.GetBoolean(6),
                            Microchip = reader.GetBoolean(7),
                            Price = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                        };
                        puppies.Add(puppy);

                    }

                }
                else
                {
                    throw new ArgumentException("Puppy record not found");
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

            return puppies;
        }

        public List<Puppy> SelectAllPuppies()
        {
            List<Puppy> puppies = new List<Puppy>();

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_all_puppies", conn);

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
                        var puppy = new Puppy()
                        {
                            PuppyID = reader.GetString(0),
                            BreedID = reader.GetString(1),
                            LitterID = reader.GetString(2),
                            MedicalRecordID = reader.GetString(3),
                            Image = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Gender = reader.GetString(5),
                            Adopted = reader.GetBoolean(6),
                            Microchip = reader.GetBoolean(7),
                            Price = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                        };
                        puppies.Add(puppy);
                    }
                }
                else
                {
                    throw new ArgumentException("Puppy record not found");
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

            return puppies;
        }


        public int UpdatePuppy(string puppyID, bool oldAdopted, bool newAdopted)
        {
            int result = 0;

            //connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_update_puppy", conn);
            //type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@PuppyID", SqlDbType.NVarChar, 50).Value = puppyID;
            cmd.Parameters.Add("@OldAdopted", SqlDbType.Bit).Value = oldAdopted;
            cmd.Parameters.Add("@NewAdopted", SqlDbType.Bit).Value = newAdopted;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

    }



}
