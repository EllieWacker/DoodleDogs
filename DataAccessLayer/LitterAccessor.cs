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
    public class LitterAccessor : ILitterAccessor
    {
        public int DeleteLitterByLitterID(string litterID)
        {
            int result = 0;

            // connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_delete_litter_by_litterID", conn);
            // type
            cmd.CommandType = CommandType.StoredProcedure;
            // parameters
            cmd.Parameters.Add("@LitterID", SqlDbType.NVarChar, 50);
            // values
            cmd.Parameters["@LitterID"].Value = litterID;
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
        public int UpdateLitter(string litterID, string fatherDogID, string motherDogID, string image, DateTime dateOfBirth, DateTime goHomeDate, int numberPuppies)
        {
            int result = 0;

            // Connection
            var conn = DBConnection.GetConnection();

            // Command
            var cmd = new SqlCommand("sp_update_litter_by_litterid", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Parameters
            cmd.Parameters.Add("@LitterID", SqlDbType.NVarChar, 50).Value = litterID;
            cmd.Parameters.Add("@FatherDogID", SqlDbType.NVarChar, 50).Value = fatherDogID;
            cmd.Parameters.Add("@MotherDogID", SqlDbType.NVarChar, 50).Value = motherDogID;
            cmd.Parameters.Add("@Image", SqlDbType.NVarChar, 30).Value = image;
            cmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = dateOfBirth;
            cmd.Parameters.Add("@GoHomeDate", SqlDbType.DateTime).Value = goHomeDate;
            cmd.Parameters.Add("@NumberPuppies", SqlDbType.Int).Value = numberPuppies;

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



        public int InsertLitter(string litterID, string fatherDogID, string motherDogID, string image, DateTime dateOfBirth, DateTime goHomeDate, int numberPuppies)
        {
            int result = 0;
            //connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_insert_Litter", conn);
            //type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@LitterID", SqlDbType.NVarChar, 50).Value = litterID;
            cmd.Parameters.Add("@FatherDogID", SqlDbType.NVarChar, 50).Value = fatherDogID;
            cmd.Parameters.Add("@MotherDogID", SqlDbType.NVarChar, 50).Value = motherDogID;
            cmd.Parameters.Add("@Image", SqlDbType.NVarChar, 30).Value = image;
            cmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = dateOfBirth;
            cmd.Parameters.Add("@GoHomeDate", SqlDbType.DateTime).Value = goHomeDate;
            cmd.Parameters.Add("@NumberPuppies", SqlDbType.Int).Value = numberPuppies;

            try
            {
                conn.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }


        public Litter SelectLitterByLitterID(string litterID)
        {
            Litter litter = null;

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_litter_by_litterID", conn);

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
                    reader.Read();
                    litter = new Litter()
                    {
                        LitterID = reader.GetString(0),
                        FatherDogID = reader.GetString(1),
                        MotherDogID = reader.GetString(2),
                        Image = reader.IsDBNull(2) ? null : reader.GetString(3),
                        DateOfBirth = reader.GetDateTime(4),
                        GoHomeDate = reader.GetDateTime(5),
                        NumberPuppies = reader.GetInt32(6)
                    };
                }
                else
                {
                    throw new ArgumentException("Litter record not found");
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

            return litter;
        }

        public List<Litter> SelectAllLitters()
        {
            List<Litter> litters = new List<Litter>();

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_all_litters", conn);

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
                        var litter = new Litter()
                        {
                            LitterID = reader.GetString(0),
                            FatherDogID = reader.GetString(1),
                            MotherDogID = reader.GetString(2),
                            Image = reader.IsDBNull(2) ? null : reader.GetString(3),
                            DateOfBirth = reader.GetDateTime(4),
                            GoHomeDate = reader.GetDateTime(5),
                            NumberPuppies = reader.GetInt32(6)
                        };
                        litters.Add(litter);
                    }
                }
                else
                {
                    throw new ArgumentException("Litter record not found");
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

            return litters;
        }

    }
}
