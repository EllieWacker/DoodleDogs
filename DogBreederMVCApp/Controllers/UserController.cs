using DataDomain;
using LogicLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DogBreederMVCApp.Controllers
{
    public class UserController : Controller
    {
        private IUserManager _userManager = new UserManager();
        private List<User> _users;

        // GET: UserController1
        [Authorize(Roles = "Admin")]
        public ActionResult Index() // admin view of all users
        {
            _users = _userManager.SelectAllUsers();
            return View(_users);
        }

    }
}
