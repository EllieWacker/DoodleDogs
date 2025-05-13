using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataDomain;
using LogicLayer;
using DogBreederMVCApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVCApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace DogBreederMVCApp.Controllers
{ 
    // update-database
    public class AdoptionController : Controller
    {
        private IAdoptionManager _adoptManager = new AdoptionManager();
        private IUserManager _userManager = new UserManager();
        private IPuppyManager _puppyManager = new PuppyManager();
        private IMedicalRecordManager _medicalRecordManager = new MedicalRecordManager();
        private List<Adoption> _adoptions;
        private List<DataDomain.User> _users;
        private ILitterManager _litManager = new LitterManager();

        private UserManager<IdentityUser> _identityUserManager;

        public AdoptionController(
             UserManager<IdentityUser> userManager)
        {
            // new to instantiate identity manager classes
            _identityUserManager = userManager;
        }

        // GET: AdoptionController
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            _adoptions = _adoptManager.GetAllAdoptions();
            _users = _userManager.SelectAllUsers();
            var model = new AdoptionUserViewModel
            {
                Adoptions = _adoptions,
                Users = _users
            };
          
            return View(model);
        }
        private void populateDropDowns()
        {
            List<string> Statuses = new List<string>()
            {
                "In Progress",
                "Cancelled",
                "Adopted"
            };
            ViewBag.Statuses = Statuses;
        }


        // GET: AdoptionController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            _adoptions = _adoptManager.GetAllAdoptions();
            populateDropDowns();
            var adoption = _adoptions.FirstOrDefault(a => a.AdoptionID == id);
            return View(adoption);
        }

        // POST: AdoptionController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Adoption adoption)
        {
            populateDropDowns();
            try
            {
                if (ModelState.IsValid)
                {
                    _adoptManager.UpdateAdoption(adoption.AdoptionID, "In Progress", adoption.Status);
                    TempData["SuccessMessage"] = "Status updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                return View(adoption);
            }
            catch
            {
                return View(adoption);
            }
        }


        public ActionResult Details(int userID)
        {
            var name = User.Identity.Name;

            //Gets a list of adoptions for the user that aren't cancelled
            List<Adoption> adoptions = _adoptManager.GetAllAdoptions().Where(a => a.UserID == userID && a.Status != "Cancelled").ToList();
            List<Puppy> puppies = new List<Puppy>();
            List<Litter> _litters = new List<Litter>();
            foreach (Adoption adoption in adoptions)
            {
                Puppy puppy = _puppyManager.GetPuppyByPuppyID(adoption.PuppyID);
                puppies.Add(puppy);

                Litter litter = _litManager.SelectLitterByLitterID(puppy.LitterID);
                _litters.Add(litter);
            }

            var model = new PuppyAdoptionsViewModel
            {
                Puppies = puppies,
                Adoptions = adoptions,
                Litters = _litters
            };

            return View(model);
        }


    }
}
