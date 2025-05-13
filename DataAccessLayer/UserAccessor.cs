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
using System.Reflection.Metadata.Ecma335;

namespace DataAccessLayer
{
    public class UserAccessor : IUserAccessor
    {
        public UserVM SelectUserByEmail(string email)
        {
            UserVM user = null;

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_user_by_email", conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            //parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250);

            //values
            cmd.Parameters["@Email"].Value = email;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    user = new UserVM()
                    {
                        UserID = reader.GetInt32(0),
                        GivenName = reader.GetString(1),
                        FamilyName = reader.GetString(2),
                        PhoneNumber = reader.GetString(3),
                        Email = email,
                        Active = reader.GetBoolean(5)
                    }; 
                }
                else
                {
                    throw new ArgumentException("User record not found");
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

            return user;
        }

        public int SelectUserCountByEmailAndPasswordHash(string email, string passwordHash)
        {
            int count = 0;
            
            var conn = DBConnection.GetConnection();

            var cmd = new SqlCommand("sp_authenticate_user", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            try
            {
                conn.Open();

                var result = cmd.ExecuteScalar();
                count = Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return count;
        }

        public List<string> SelectRolesByUserID(int userID)
        {
            List<string> roles= new List<string>();
            //connection
            var conn = DBConnection.GetConnection();
            //command
            var cmd = new SqlCommand("sp_select_roles_by_UserID", conn);
            //command type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            //values
            cmd.Parameters["@UserID"].Value = userID;

            try
            {
                conn.Open() ;
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(reader.GetString(1));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving roles from database.", ex);
            }
            finally
            {
                conn.Close();
            }
            return roles;
        }

        public int UpdatePasswordHashByEmail(string email, string oldPasswordHash, string newPasswordHash)
        {
            int result = 0;

            //connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_update_passwordhash_by_email", conn);
            //type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250).Value = email;
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);
            //values
            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;

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

        public int InsertUser(string givenName, string familyName, string phoneNumber, string email)
        {
            int result = 0;
            //connection
            var conn = DBConnection.GetConnection();
            // command
            var cmd = new SqlCommand("sp_insert_user", conn);
            //type
            cmd.CommandType = CommandType.StoredProcedure;
            //parameters
            cmd.Parameters.Add("@GivenName", SqlDbType.NVarChar,50).Value = givenName;
            cmd.Parameters.Add("@FamilyName", SqlDbType.NVarChar, 100).Value = familyName;
            cmd.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 11).Value = phoneNumber;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250).Value = email;

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
        public List<User> SelectAllUsers()
        {
            List<User> users = new List<User>();

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_all_users", conn);

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
                        var user = new User()
                        {
                            UserID = reader.GetInt32(0),
                            GivenName = reader.GetString(1),
                            FamilyName = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            Email = reader.GetString(4),
                            Active = reader.GetBoolean(5)
                        };
                        users.Add(user);
                    }
                }
                else
                {
                    throw new ArgumentException("User record not found");
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

            return users;
        }

    }
}