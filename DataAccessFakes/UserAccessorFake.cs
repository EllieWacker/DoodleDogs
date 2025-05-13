using DataAccessInterfaces;
using DataDomain;
using DataAccessFakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace DataAccessFakes
{
    public class UserAccessorFake : IUserAccessor
    {
        private List<UserVM> _users;
        private List<UserRole> _userRoles;
        public UserAccessorFake()
        {
            _users = new List<UserVM>();
            _userRoles = new List<UserRole>();
            _userRoles.Add(new UserRole() { UserID = 1, RoleID = "Role1" });
            _userRoles.Add(new UserRole() { UserID = 1, RoleID = "Role2" });
            _userRoles.Add(new UserRole() { UserID = 2, RoleID = "Role3" });

            _users.Add(new UserVM()
            {
                UserID = 1,
                GivenName = "Test1",
                FamilyName = "Test1",
                Email = "test1@test.com",
                PhoneNumber = "1234567890",
                PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                Active = true
            });
            _users.Add(new UserVM()
            {
                UserID = 2,
                GivenName = "Test2",
                FamilyName = "Test2",
                Email = "test2@test.com",
                PhoneNumber = "1234567890",
                PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                Active = true
            });
            _users.Add(new UserVM()
            {
                UserID = 3,
                GivenName = "Test3",
                FamilyName = "Test3",
                Email = "test3@test.com",
                PhoneNumber = "1234567890",
                PasswordHash = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                Active = false
            });
        }

        public UserVM SelectUserByEmail(string email)
        {
            foreach (UserVM emp in _users)
            {
                if (emp.Email == email)
                {
                    return emp;
                }
            }
            throw new ArgumentException("User record not found");
        }

        public int SelectUserCountByEmailAndPasswordHash(string email, string passwordHash)
        {
            int count = 0;
            foreach (var user in _users)
            {
                if (user.Email == email && user.PasswordHash == passwordHash && user.Active == true)
                {
                    count++;
                }
            }
            return count;
        }

        public List<string> SelectRolesByUserID(int userID)
        {
            List<string> roles = new List<string>();

            foreach (var userRole in _userRoles)
            {
                if (userRole.UserID == userID)
                {
                    roles.Add(userRole.RoleID);
                }
            }

            return roles;
        }

        public int UpdatePasswordHashByEmail(string email, string oldPasswordHash, string newPasswordHash)
        {
            int count = 0;
            for (int i = 0; i < _users.Count(); i++)
            {
                if (_users[i].Email == email && _users[i].PasswordHash == oldPasswordHash)
                {
                    _users[i].PasswordHash = newPasswordHash;
                    count++;
                }
            }
            if (count == 0)
            {
                throw new ArgumentException("User record not found");
            }
            return count;
        }


        public int InsertUser(string givenName, string familyName, string phoneNumber, string email)
        {
            int result = 0;
            var _user = new UserVM()
            {
                GivenName = givenName,
                FamilyName = familyName,
                PhoneNumber = phoneNumber,
                Email = email,
                Active = true
            };
            if (_users.Count > 0)
            {
                // Get the last UserID and increment it
                _user.UserID = _users.Count + 999999;
            }
            else
            {
                // If no users, start from 1000000
                _user.UserID = 1000000;
            }

            _users.Add(_user);
            result = _user.UserID;

            if (result == 0)
            {
                throw new ArgumentException("Unable to create user");
            }
            // Return the new UserID
            return result;
        }

        public List<User> SelectAllUsers()
        {
            List<User> users = new List<User>();
            foreach (var user in _users)
            {
                users.Add(user);
            }
            return users;
        }
    }
    internal class UserRole
    {
        public int UserID { get; set; }
        public string RoleID { get; set; }
    }
}
