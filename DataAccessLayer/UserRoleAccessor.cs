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
    public class UserRoleAccessor : IUserRoleAccessor
    {
        public string InsertRoles(string userRoleID, int userID, string description)
        {
            string result = "none";
            //connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_insert_roles", conn);
            //type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@UserRoleID", SqlDbType.NVarChar, 50).Value = userRoleID;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 250).Value = description;

            try
            {
                conn.Open();
                result = cmd.ExecuteScalar().ToString();
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

        public int UpdateUserRole(int userID, string oldUserRoleID, string newUserRoleID)
        {
            int result = 0;

            //connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_update_user_role_by_userid", conn);
            //type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("@OldUserRoleID", SqlDbType.NVarChar, 50).Value = oldUserRoleID;
            cmd.Parameters.Add("@NewUserRoleID", SqlDbType.NVarChar, 50).Value = newUserRoleID;

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
