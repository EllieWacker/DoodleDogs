using DataDomain;
using LogicLayer;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

// this class was created to allow an easy way to get a legacy access token for use on pages
namespace DogBreederMVCApp.Models
{
    public class AccessToken
    {
        DataDomain.User _legacyUser;
        List<string> _roles;
        UserRoleManager _userRoleManager = new UserRoleManager();

        public AccessToken(string email)
        {
            _roles = new List<string>();
            LogicLayer.UserManager userManager = new LogicLayer.UserManager();

            List<DataDomain.User> _users = userManager.SelectAllUsers();
            if (email == "" || email == null) 
            {
                _legacyUser = new DataDomain.User()
                {
                    UserID =  0, 
                    Email = "",
                    FamilyName = "",
                    GivenName = "",
                    PhoneNumber = ""
                };
                _roles = new List<string>();
                return;
            }

            try // try to match the user to one already in the database. 
            {
                _legacyUser = userManager.RetrieveUserByEmail(email);
                if (_legacyUser != null)
                {
                    _roles = userManager.GetRolesForUser(_legacyUser.UserID);
                }
            }
            catch
            {
                Console.WriteLine("Error retrieving legacy user.");
                _legacyUser = new DataDomain.User()
                {
                    UserID = 0,
                    Email = "",
                    FamilyName = "",
                    GivenName = "",
                    PhoneNumber = ""
                };
                _roles = new List<string>();
            }
        }
            

        public bool IsSet { get { return _legacyUser.UserID != 0 && _legacyUser != null; } }
        public int userID { get { return _legacyUser.UserID; } }
        public string GivenName { get { return _legacyUser.GivenName; } }
        public string FamilyName { get { return _legacyUser.FamilyName; } }
        public List<String> Roles { get { return _roles; } }
    }
}
