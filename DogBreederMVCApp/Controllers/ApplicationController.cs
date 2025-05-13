using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataDomain;
using LogicLayer;
using Microsoft.AspNetCore.Authorization;
using DogBreederMVCApp.Models;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace DogBreederMVCApp.Controllers
{
    public class ApplicationController : Controller
    {
        private IApplicationManager _manager = new ApplicationManager();
        private IUserManager _userManager = new UserManager();
        private IUserRoleManager _userRoleManager = new UserRoleManager();
        private User _user = new User();

        private UserManager<IdentityUser> identityUserManager;

        public ApplicationController(UserManager<IdentityUser> userManager)
        {
            // new to instantiate identity manager classes
            identityUserManager = userManager;
        }


        // GET: ApplicationAccessor
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var applications = _manager.GetAllApplications();
            return View(applications);
        }

        // GET: ApplicationAccessor/Create
        public ActionResult Create()
        {
            var accessToken = new AccessToken(User.Identity.Name);

            // I was having issues with the aspNetUserRoles and Authorize here so I manually am checking and redirecting based on the roles in my database instead.
            if (accessToken.Roles.Contains("User") || accessToken.Roles.Contains("Adopter")){
                populateDropDowns();
                return View();
            }
            else
            {
                TempData["ErrorMessage"] = "You are not authorized to create an application. Please log in.";
                return RedirectToAction("Index", "Home");
            }
        }

        private void populateDropDowns()
        {
            List<string> BreedOptions = new List<string>()
            {
                "Mini Aussiedoodle",
                "Mini Goldendoodle",
                "Cockapoo"
            };

            List<string> GenderOptions = new List<string>()
            {
                "Male",
                "Female",
                "No Preference"
            };

            List<string> ContactOptions = new List<string>()
            {
                "Text",
                "Call",
                "Email"
            };

            ViewBag.BreedOptions = BreedOptions;
            ViewBag.GenderOptions = GenderOptions;
            ViewBag.ContactOptions = ContactOptions;
        }

        // POST: ApplicationAccessor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DataDomain.Application application)
        {

            var accessToken = new AccessToken(User.Identity.Name);

            populateDropDowns();
            try
            {
                if (!application.FullName.ToLower().Contains(accessToken.FamilyName.ToLower()))
                {
                    ModelState.AddModelError("FullName", "Your last name must match your account name.");
                }
                if (application.Age < 18)
                {
                    ModelState.AddModelError("Age", "You must be over 18 to create an application.");
                }
                if(application.Age > 100 || application.Age < 0)
                {
                    ModelState.AddModelError("Age", "Invalid age");
                }

                if (ModelState.IsValid)
                {
                    application.UserID = accessToken.userID;
                    _manager.InsertApplication(application.UserID, application.FullName, application.Age, application.Renting, application.Yard, application.DesiredBreed, application.DesiredGender, application.PreferredContact, application.Status, application.Comment);
                    ViewBag.SuccessMessage = "Application Created Successfully";
                    return View(application);
                }
                else
                {
                    return View(application);
                }
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: ApplicationAccessor/Approve/5
        [Authorize(Roles = "Admin")]
        public ActionResult Approve(int id)
        {
            var application = _manager.GetAllApplications().FirstOrDefault(_application => _application.ApplicationID == id);
            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id, DataDomain.Application application)
        {
            var userRoleManager = new UserRoleManager();
            List<string> _userRoleList = _userManager.GetRolesForUser(application.UserID);
            _user = _userManager.SelectAllUsers().FirstOrDefault(_user => _user.UserID == application.UserID); // finds the user that matches that application
            try
            {
                if (_userRoleList.Contains("User"))
                {
                    string oldUserRole = "User";
                    string newUserRole = "Adopter";
                    userRoleManager.UpdateUserRole(_user.UserID, oldUserRole, newUserRole);
                }
                _manager.UpdateApplicationStatus(application.ApplicationID, false, true);
                TempData["SuccessMessage"] = "Application Approved";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong while updating the user role.";
                return View(application);
            }
        }

    }
}
