using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessInterfaces;
using DataAccessLayer;
using DataDomain;

namespace LogicLayer
{
    public class UserManager : IUserManager
    {
        private IUserAccessor _userAccessor;

        public UserManager()
        {
           _userAccessor = new UserAccessor();

        }

        public UserManager(IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;

        }

        public bool AuthenticateUser(string email, string password)
        {
            bool result = false;

            password = HashSHA256(password);
            result = (1 == _userAccessor.SelectUserCountByEmailAndPasswordHash(email, password));


            return result;
        }

        public List<string> GetRolesForUser(int userId) {
            List<string> roles = null;

            try
            {
                roles = _userAccessor.SelectRolesByUserID(userId);
                Console.WriteLine($"Roles for User {userId}: {string.Join(", ", roles)}"); // Debugging the returned roles
                if (roles.Count == 0)
                {
                    throw new Exception("No roles were retrieved from the database");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("No roles were found.", ex);
            }

            return roles;
        }

        public string HashSHA256(string password)
        {
            if (password == "" || password == null)
            {
                throw new ArgumentException("Missing input");
            }

            string hashValue = null;

            byte[] data;

            using (SHA256 sha256provider = SHA256.Create())
            {
                data = sha256provider.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            var s = new StringBuilder();

            for(int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            hashValue = s.ToString();

            return hashValue;
        }

        public UserVM LoginUser(string email, string password)
        {
            UserVM userVM = null;

            try
            {
                if(AuthenticateUser(email, password))
                {
                    userVM = (UserVM)RetrieveUserByEmail(email);
                    if (userVM != null)
                    {
                        userVM.Roles = GetRolesForUser(userVM.UserID);
                    }
                }

                else
                {
                    throw new ArgumentException("Bad password or email");
                }
            }
            catch (Exception)
            {
                throw; 
            }

            return userVM;
        }


        public User RetrieveUserByEmail(string email)
        {
            User user = null;
            try
            {
                user = _userAccessor.SelectUserByEmail(email);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Login Failed", ex);
            }
            return user;
        }

        public bool UpdatePassword(string email, string oldPassword, string newPassword)
        {
            bool result = false;
            oldPassword = HashSHA256(oldPassword);
            newPassword = HashSHA256(newPassword);
            try
            {
                result = (1 == _userAccessor.UpdatePasswordHashByEmail(email, oldPassword, newPassword));
                if(!result)
                {
                    throw new ApplicationException("Duplicate User Records Found");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed.", ex);
            }

            return result;
        }

        public int InsertUser(string givenName, string familyName, string phoneNumber, string email)
        {
            int result = 0;
            try
            {
                result =(_userAccessor.InsertUser(givenName, familyName, phoneNumber, email));
                if (result == 0)
                {
                    throw new ApplicationException("Insert User Failed");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Insert failed.");
            }
            return result;
        }
        public List<User> SelectAllUsers()
        {
            List<User> users = new List<User>();
            try
            {
                users = _userAccessor.SelectAllUsers();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("User list not found", ex);
            }
            return users;
        }
    }
}
