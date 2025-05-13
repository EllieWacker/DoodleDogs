using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IUserRoleManager
    {
        string InsertUserRole(string userRoleID, int userID, string description);
        int UpdateUserRole(int userID, string oldUserRoleID, string newUserRoleID);
    }
}
