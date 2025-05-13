using DataDomain;
using DogBreederMVCApp.Models;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Azure.Core;

namespace DogBreederMVCApp.Controllers
{
    public class HomeController : Controller
    {
        // identity
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;


        private readonly ILogger<HomeController> _logger;
        private ITestimonialManager _testManager = new TestimonialManager();

        // changed to acquire identity manager classes (injected by request pipeline, no ninject needed)
        public HomeController(ILogger<HomeController> logger,
               UserManager<IdentityUser> userManager,
               SignInManager<IdentityUser> signInManager)
        {
            // new to instantiate identity manager classes
            _userManager = userManager;
            _signInManager = signInManager;

            _logger = logger;
        }

        private Models.AccessToken _accessToken = new Models.AccessToken("");

        private void getAccessToken()
        {
            if (_signInManager.IsSignedIn(User))
            {
                string email = User.Identity.Name;
                try
                {
                    _accessToken = new Models.AccessToken(email);
                }
                catch(Exception ex)
                { 
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                return;
            }
        }


        public async Task<IActionResult> IndexAsync()
        {
            getAccessToken();
            if (_accessToken.IsSet)
            {

                var id = _userManager.GetUserId(User);
                var users = _userManager.Users;
                var u = users.FirstOrDefault(u => u.Id == id);

                // ApplicationDbContext context = _userManager.Users.
                foreach (var role in _accessToken.Roles)
                {
                    if (!User.IsInRole(role))
                    {
                        await _userManager.AddToRoleAsync(u, role);
                    }
                }
                // context.SaveChanges();
            }

            var testimonials = _testManager.GetAllTestimonials();
            return View(testimonials);
        }

        public IActionResult AboutUs()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            // if all is well, logging in with Admin (password = P@ssw0rd) will show admin@company.com for name and Admin for role.
            var name = User.Identity.Name;
            var role = User.IsInRole("Admin");
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Approve()
        {
            // if all is well, logging in with Admin (password = P@ssw0rd) will show admin@company.com for name and Admin for role.
            var name = User.Identity.Name;
            var role = User.IsInRole("Admin");
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
