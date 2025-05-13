using DataAccessInterfaces;
using DataAccessLayer;
using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class UserRoleManager : IUserRoleManager
    {
        private IUserRoleAccessor _userRoleAccessor;

        public UserRoleManager()
        {
            _userRoleAccessor = new UserRoleAccessor();

        }

        public UserRoleManager(IUserRoleAccessor userroleAccessor)
        {
            _userRoleAccessor = userroleAccessor;

        }
        public string InsertUserRole(string userRoleID, int userID, string description)
        {
            string result = "None";
            try
            {
                result = _userRoleAccessor.InsertRoles(userRoleID, userID, description);
                if (result.Equals("None")) 
                {
                    throw new ApplicationException("Insert User Role Failed");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Insert failed.", ex);
            }


            return result;
        }

        public int UpdateUserRole(int userID, string oldUserRoleID, string newUserRoleID)
        {
            int result = 0;
            try
            {
                result = (_userRoleAccessor.UpdateUserRole(userID, oldUserRoleID, newUserRoleID));
                if (result == 0)
                {
                    throw new ApplicationException("Role not updated");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed.", ex);
            }

            return result;
        }

    }
}
