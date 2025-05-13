using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IUserAccessor
    {
        int SelectUserCountByEmailAndPasswordHash(string email, string password);
        UserVM SelectUserByEmail(string email);
        
        List<string> SelectRolesByUserID(int userID);
        int UpdatePasswordHashByEmail(string email, string oldPasswordHash, string newPasswordHash);
        int InsertUser(string givenName, string familyName, string phoneNumber, string email);
        List<User> SelectAllUsers();

    }
}
