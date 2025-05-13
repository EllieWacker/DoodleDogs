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
using System.Reflection.PortableExecutable;

namespace DataAccessLayer
{
    public class ApplicationAccessor : IApplicationAccessor
    {
        public List<Application> SelectApplicationsByUserID(int userID)
        {
            List<Application> applications = new List<Application>();

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_applications_by_userID", conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            //parameters
            cmd.Parameters.Add("@UserID", SqlDbType.Int);

            //values
            cmd.Parameters["@UserID"].Value = userID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var application = new Application()
                        {
                            ApplicationID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            FullName = reader.GetString(2),
                            Age = reader.GetInt32(3),
                            Renting = reader.GetBoolean(4),
                            Yard = reader.GetBoolean(5),
                            DesiredBreed = reader.GetString(6),
                            DesiredGender = reader.GetString(7),
                            PreferredContact = reader.GetString(8),
                            Status = reader.GetBoolean(9),
                            Comment = reader.IsDBNull(10) ? null : reader.GetString(10),
                        };
                        applications.Add(application);
                    }
                }
                else
                {
                    throw new ArgumentException("Application record not found");
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

            return applications;
        }

        public int InsertApplication(int userID, string fullName, int age, bool renting, bool yard, string desiredBreed, string desiredGender, string preferredContact, bool status, string comment)
        {
            int result = 0;
            //connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_insert_application", conn);
            //type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 50).Value = fullName;
            cmd.Parameters.Add("@Age", SqlDbType.Int).Value = age;
            cmd.Parameters.Add("@Renting", SqlDbType.Bit).Value = renting;
            cmd.Parameters.Add("@Yard", SqlDbType.Bit).Value = yard;
            cmd.Parameters.Add("@DesiredBreed", SqlDbType.NVarChar, 50).Value = desiredBreed;
            cmd.Parameters.Add("@DesiredGender", SqlDbType.NVarChar, 50).Value = desiredGender;
            cmd.Parameters.Add("@PreferredContact", SqlDbType.NVarChar, 50).Value = preferredContact;
            cmd.Parameters.Add("@Status", SqlDbType.Bit).Value = status;
            cmd.Parameters.Add("@Comment", SqlDbType.NVarChar, 400).Value = comment;

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

        public List<Application> SelectAllApplications()
        {
            List<Application> applications = new List<Application>();

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_select_all_applications", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    Application b = new Application();
                    b.ApplicationID = r.GetInt32(0);
                    b.UserID = r.GetInt32(1);
                    b.FullName = r.GetString(2);
                    b.Age = r.GetInt32(3);
                    b.Renting = r.GetBoolean(4);
                    b.Yard = r.GetBoolean(5);
                    b.DesiredBreed = r.GetString(6);
                    b.DesiredGender = r.GetString(7);
                    b.PreferredContact = r.GetString(8);
                    b.Status = r.GetBoolean(9);
                    b.Comment = r.IsDBNull(10) ? null : r.GetString(10);
                    applications.Add(b);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return applications;
        }
        public int UpdateApplicationStatus(int applicationID, bool oldStatus, bool newStatus)
        {
            int result = 0;

            //connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_update_application_status", conn);
            //type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationID;
            cmd.Parameters.Add("@OldStatus", SqlDbType.Bit).Value = oldStatus;
            cmd.Parameters.Add("@NewStatus", SqlDbType.Bit).Value = newStatus;

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
