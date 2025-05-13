using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessFakes;
using DataDomain;

namespace LogicLayer
{
    public interface IUserManager
    {
        string HashSHA256(string password);
        bool AuthenticateUser(string email, string password);
        public User RetrieveUserByEmail(string email);
        public List<String> GetRolesForUser(int userId);
        public UserVM LoginUser(string email, string password);
        public bool UpdatePassword(string email, string oldPassword, string newPassword);
        public int InsertUser(string givenName, string familyName, string phoneNumber, string email);
        public List<User> SelectAllUsers();

    }
}
