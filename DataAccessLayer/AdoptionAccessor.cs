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
    public class AdoptionAccessor : IAdoptionAccessor
    {
        public List<Adoption> SelectAllAdoptions()
        {
            List<Adoption> adoptions = new List<Adoption>();

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_all_adoptions", conn);

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
                        var adoption = new Adoption()
                        {
                            AdoptionID = reader.GetInt32(0),
                            ApplicationID = reader.GetInt32(1),
                            PuppyID = reader.GetString(2),
                            UserID = reader.GetInt32(3),
                            Status = reader.GetString(4)
                        };
                        adoptions.Add(adoption);
                    }
                }
                else
                {
                    throw new ArgumentException("Adoption record not found");
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

            return adoptions;
        }

        public int InsertAdoption(int applicationID, string puppyID, int userID, string status)
        {
            int result = 0;
            //connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_insert_adoption", conn);
            //type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationID;
            cmd.Parameters.Add("@PuppyID", SqlDbType.NVarChar, 50).Value = puppyID;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = status;

            try
            {
                conn.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
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

        public int UpdateAdoption(int adoptionID, string oldStatus, string newStatus)
        {
            int result = 0;

            //connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_update_adoption_status_by_id", conn);
            //type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@AdoptionID", SqlDbType.Int).Value = adoptionID;
            cmd.Parameters.Add("@OldStatus", SqlDbType.NVarChar, 50).Value = oldStatus;
            cmd.Parameters.Add("@NewStatus", SqlDbType.NVarChar, 50).Value = newStatus;

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
