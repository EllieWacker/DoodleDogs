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
using Microsoft.CodeAnalysis.CSharp;

namespace DogBreederMVCApp.Controllers
{
    public class DogController : Controller
    {
        private IPuppyManager _manager = new PuppyManager();
        private IMedicalRecordManager _medManager = new MedicalRecordManager();
        private IAdoptionManager _adoptManager = new AdoptionManager();
        private IApplicationManager _applicationManager = new ApplicationManager();
        private IUserManager _userManager = new UserManager();
        private UserVM _user;
        private List<String> _userRoles = new List<String>();
        private ILitterManager _litterManager = new LitterManager();
        private List<Litter> _litters = new List<Litter>();


        private UserManager<IdentityUser> identityUserManager;
        public DogController(UserManager<IdentityUser> userManager)
        {
            // new to instantiate identity manager classes
            identityUserManager = userManager;
        }



        // GET: DogController
        public ActionResult Index(string breedID)
        {
            var accessToken = new AccessToken(User.Identity.Name);

            ViewBag.UserID = accessToken.userID;
            List<DataDomain.Application> applications = new List<DataDomain.Application>();

            List<Adoption> adoptions = _adoptManager.GetAllAdoptions();
            try
            {
                applications = _applicationManager.SelectApplicationsByUserID(accessToken.userID);
            }catch{

            }

            if (accessToken.IsSet)
            {
                _userRoles = accessToken.Roles;

                if (_userRoles.Contains("adopter") || _userRoles.Contains("Adopter"))
                {
                    ViewBag.IsAdopter = true;
                    // checks for approved applications
                    var approvedApplications = applications.Where(app => app.Status == true).ToList();

                    // gets all applicationIDs in adoptions
                    var adoptedApplicationIDs = adoptions.Select(adopt => adopt.ApplicationID).ToList();

                    bool canAdopt = false;

                    // Sees if the user has any unused applications
                    foreach (var app in approvedApplications)
                    {
                        if (!adoptedApplicationIDs.Contains(app.ApplicationID))
                        {
                            canAdopt = true;
                            break;
                        }
                    }
                    ViewBag.CanAdopt = canAdopt;
                }
                else
                {
                    ViewBag.CanAdopt = false;
                }
            }

            var puppies = _manager.SelectPuppiesByBreedID(breedID);
            
            return View(puppies);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult IndexBasic()
        {
            var puppies = _manager.GetAllPuppies();
            return View(puppies);
        }


        private void populateDropDowns()
        {
            _litters = _litterManager.GetAllLitters();
            List<string> BreedIDs = new List<string>()
            {
                "Mini Aussiedoodle",
                "Mini Goldendoodle",
                "Cockapoo"
            };

            List<string> LitterIDs = new List<string>();

            foreach (var litter in _litters)
            {
                LitterIDs.Add(litter.LitterID);
            }

            List<string> MedicalRecordIDs = new List<string>()
            {
                "LukeWacker1",
                "AmyMeyer1",
                "SusanFaley1",
                "CarterSmith1"
            };
            List<string> Genders = new List<string>()
            {
                "Male",
                "Female"
            };

            ViewBag.BreedIDs = BreedIDs;
            ViewBag.LitterIDs = LitterIDs;
            ViewBag.MedicalRecordIDs = MedicalRecordIDs;
            ViewBag.Genders = Genders;
        }

        // GET: DogController/Details/5
        public ActionResult Details(string puppyID, string medicalRecordID)
        {
            var puppy = _manager.GetPuppyByPuppyID(puppyID);
            var medRecord = _medManager.SelectMedicalRecordByMedicalRecordID(medicalRecordID);
            var accessToken = new AccessToken(User.Identity.Name);
            _userRoles = accessToken.Roles;

            if (_userRoles.Contains("Admin"))
            {
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }

            var model = new PuppyDetailsViewModel
            {
                Puppy = puppy,
                MedicalRecord = medRecord
            };

            return View(model);
        }

        // GET: DogController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            populateDropDowns();
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Puppy puppy, IFormFile NewImage)
        {
            populateDropDowns();
            try 
            {
                // If both the new image and old image are null the user has to add a new one
                if (NewImage == null || NewImage.Length == 0)
                {
                    if (string.IsNullOrEmpty(puppy.Image))
                    {
                        ModelState.AddModelError("Image", "You must pick an image.");
                    }
                }
                // Validation for if there is a new image
                else
                {
                    if (NewImage.FileName.Length < 5)
                    {
                        ModelState.AddModelError("Image", "Image file name must contain more than 5 characters.");
                    }
                    if (NewImage.FileName.Contains(' '))
                    {
                        ModelState.AddModelError("Image", "The image file name must not include spaces.");
                    }
                }
                if (_manager.GetAllPuppies().Any(existingPuppy => puppy.PuppyID == existingPuppy.PuppyID))
                {
                    ModelState.AddModelError("PuppyID", "A puppy with that name already exists.");
                }
                if (Convert.ToDecimal(puppy.Price) < 800)
                {
                    ModelState.AddModelError("Price", "Puppy's price must be more than $800.00");
                }
                if (Convert.ToDecimal(puppy.Price) > 1500)
                {
                    ModelState.AddModelError("Price", "Puppy's price must be less than $1500.00");
                }

                if (NewImage != null && NewImage.Length > 0)
                {
                    string fileExtension = Path.GetExtension(NewImage.FileName); // gets the file extension
                    string newFileName = $"{puppy.PuppyID}{fileExtension}"; // creates new file name

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", newFileName);

                    // creates the image in the desired location and overwrites it if it already exists
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        NewImage.CopyTo(stream);
                    }
                    puppy.Image = newFileName;
                }
                if (!string.IsNullOrEmpty(puppy.Image))
                {
                    ModelState.Remove("NewImage");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _manager.InsertPuppy(puppy.PuppyID, puppy.BreedID, puppy.LitterID, puppy.MedicalRecordID, puppy.Image, puppy.Gender, puppy.Adopted, puppy.Microchip, (decimal)puppy.Price);
                        TempData["SuccessMessage"] = "Puppy created successfully!"; // shows the message in index
                        return RedirectToAction(nameof(IndexBasic));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Image", "An error occurred please try again.");
                        Console.WriteLine(ex.ToString());
                        return View(puppy);
                    }
                }
                else
                {
                    return View(puppy);
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return View();
            }
        }



        // GET: DogController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            populateDropDowns();
            var puppy = _manager.GetPuppyByPuppyID(id);
            return View(puppy);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Puppy puppy, IFormFile NewImage)
        {
            populateDropDowns();
            try 
            { 
                if (Convert.ToDecimal(puppy.Price) < 800)
                {
                    ModelState.AddModelError("Price", "Puppy's price must be more than $800.00");
                }
                if (Convert.ToDecimal(puppy.Price) > 1500)
                {
                    ModelState.AddModelError("Price", "Puppy's price must be less than $1500.00");
                }
                // If both the new image and old image are null the user has to add a new one
                if (NewImage == null || NewImage.Length == 0)
                {
                    if (string.IsNullOrEmpty(puppy.Image))
                    {
                        ModelState.AddModelError("Image", "You must pick an image.");
                    }
                }
                // Validation for if there is a new image
                else
                {
                    if (NewImage.FileName.Length < 5)
                    {
                        ModelState.AddModelError("Image", "Image file name must contain more than 2 characters.");
                    }
                    if (NewImage.FileName.Contains(' '))
                    {
                        ModelState.AddModelError("Image", "The image file name must not include spaces.");
                    }
                }
                // If there is a new image save it
                if (NewImage != null)
                {
                    string fileExtension = Path.GetExtension(NewImage.FileName); // gets the file extension
                    string newFileName = $"{puppy.PuppyID}{fileExtension}"; // creates new file name

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", newFileName);

                    // creates the image in the desired location and overwrites it if it already exists
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        NewImage.CopyTo(stream);
                    }
                    puppy.Image = newFileName;
                }
                else
                {
                    ModelState.Remove("NewImage"); // If there is no new image remove it from the model state
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _manager.UpdateEntirePuppy(puppy.PuppyID, puppy.BreedID, puppy.LitterID, puppy.MedicalRecordID, puppy.Image, puppy.Gender, puppy.Adopted, puppy.Microchip, (decimal)puppy.Price);
                        TempData["SuccessMessage"] = "Puppy updated successfully!"; // shows the message in index
                        return RedirectToAction(nameof(IndexBasic));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Image", "An error occurred please try again.");
                        return View(puppy);
                    }
                   
                }
                else
                {
                    return View(puppy);
                }
            }
            catch
            {
                return View();
            }
        }


        // GET: DogController/Adopt/5
        public ActionResult Adopt(int userID,string puppyID, string medicalRecordID)
        {
            var puppy = _manager.GetPuppyByPuppyID(puppyID);
            var user = _userManager.SelectAllUsers().FirstOrDefault(u => u.UserID == userID);
            var applications = _applicationManager.SelectApplicationsByUserID(userID);
            DataDomain.Application application = _applicationManager.GetAllApplications().FirstOrDefault();
            List <Adoption> adoptions = _adoptManager.GetAllAdoptions();
            var medRecord = _medManager.SelectMedicalRecordByMedicalRecordID(medicalRecordID);


            foreach (DataDomain.Application app in applications)
            {
                foreach(Adoption adopt in adoptions)
                {
                    if (app.ApplicationID != adopt.ApplicationID)
                    {
                        application.ApplicationID = app.ApplicationID;
                        break;
                    }
                }
            }
            
            var adoption = new Adoption
            {
                UserID = userID,
                PuppyID = puppy.PuppyID,
                ApplicationID = application.ApplicationID,
            };
            var model = new PuppyAdoptionViewModel
            {
                Puppy = puppy,
                MedicalRecord = medRecord,
                Adoption = adoption
            };
            return View(model);
        }

        // POST: DogController/Adopt/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Adopt(Adoption adoption)
        {
            List<Adoption> adoptions = _adoptManager.GetAllAdoptions();
            Puppy puppy = _manager.GetPuppyByPuppyID(adoption.PuppyID);
            try
            {
                int result = _adoptManager.InsertAdoption(adoption.ApplicationID, adoption.PuppyID, adoption.UserID, "In Progress");

                if (result > 0)
                {
                    _manager.UpdatePuppy(adoption.PuppyID, false, true);
                    TempData["SuccessMessage"] = "Adoption completed successfully!"; // shows the message in index
                    return RedirectToAction("Index", new { breedID = puppy.BreedID });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View(adoption);
            }
            return View(adoption);
        }

        // GET: DogController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            var puppy = _manager.GetPuppyByPuppyID(id);
            return View(puppy);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Puppy puppy)
        {
            Puppy _puppy = _manager.GetPuppyByPuppyID(id);

            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", _puppy.Image); // gets the image path so it can be deleted
            try
            {
                int result = _manager.DeletePuppyByPuppyID(id);

                if (result > 0)
                {
                    System.IO.File.Delete(imagePath); //delete the puppy image
                    TempData["SuccessMessage"] = "Puppy deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Delete failed.";
                }
                return RedirectToAction(nameof(IndexBasic));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View(puppy);
            }
        }
    }
}
