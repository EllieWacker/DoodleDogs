using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IUserRoleAccessor
    {
        string InsertRoles(string userRoleID, int userID, string description);
        int UpdateUserRole(int userID, string oldUserRoleID, string newUserRoleID);
    }
}
