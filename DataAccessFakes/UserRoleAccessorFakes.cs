using DataAccessInterfaces;
using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class UserRoleAccessorFake : IUserRoleAccessor
    {
        private List<DataDomain.UserRole> _userRoles;
        private List<User> _user;
        public UserRoleAccessorFake()
        {
            _user = new List<User>();
            _userRoles = new List<DataDomain.UserRole>();
            _userRoles.Add(new DataDomain.UserRole() { UserRoleID = "Helper", UserID = 1, Description = "Role1" });
            _userRoles.Add(new DataDomain.UserRole() { UserRoleID = "DogOwner", UserID = 2, Description = "Role2" });
            _userRoles.Add(new DataDomain.UserRole() { UserRoleID = "NewPet", UserID = 3, Description = "Role3" });
        }

        public string InsertRoles(string userRoleID, int userID, string description)
        {
            string result = "none";
            var _userRole = new DataDomain.UserRole()
            {
                UserRoleID = userRoleID,
                UserID = userID,
                Description = description,
            };

            _userRoles.Add(_userRole);
            result = _userRole.UserRoleID;

            if (result == "none")
            {
                throw new ArgumentException("Unable to create user role");
            }
            return result;
        }

        public int UpdateUserRole(int userID, string oldUserRoleID, string newUserRoleID)
        {
            int count = 0;
            for (int i = 0; i < _userRoles.Count(); i++)
            {
                if (_userRoles[i].UserID == userID)
                {
                    _userRoles[i].UserRoleID = newUserRoleID;
                    count++;
                }
            }
            if (count == 0)
            {
                throw new ArgumentException("User Role record not updated");
            }
            return count;
        }
    }
}
