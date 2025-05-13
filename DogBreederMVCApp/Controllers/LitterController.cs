using DataDomain;
using DogBreederMVCApp.Models;
using LogicLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace DogBreederMVCApp.Controllers
{
    public class LitterController : Controller
    {
        private ILitterManager _litManager = new LitterManager();
        private IFatherDogManager _fatherManager = new FatherDogManager();
        private IMotherDogManager _motherManager = new MotherDogManager();
        private List<Litter> litters;
        private IUserManager _userManager = new UserManager();
        private UserVM _user;
        private List<String> _userRoles = new List<String>();

        private UserManager<IdentityUser> identityUserManager;

        public LitterController(UserManager<IdentityUser> userManager)
        {
            identityUserManager = userManager;
        }


        [Authorize(Roles = "Admin")]
        public ActionResult IndexBasic()
        {
            litters = _litManager.GetAllLitters();
            return View(litters);
        }

        // GET: LitterController/Details/5
        public ActionResult Details(string litterID, string fatherDogID, string motherDogID)
        {
            var accessToken = new AccessToken(User.Identity.Name);

            _userRoles = accessToken.Roles;
            var litter = _litManager.SelectLitterByLitterID(litterID);
            var mother = _motherManager.SelectMotherDogByMotherDogID(motherDogID);
            var father = _fatherManager.SelectFatherDogByFatherDogID(fatherDogID);

            if (_userRoles.Contains("Admin"))
            {
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
            }
            var model = new LitterDetailsViewModel
            {
                Litter = litter,
                MotherDog = mother,
                FatherDog = father,
            };

            return View(model);
        }

        private void populateDropDowns()
        {
            List<string> FatherDogIDs = new List<string>()
            {
                "Harold",
                "Finn"
            };

            List<string> MotherDogIDs = new List<string>()
            {
                "Clemmy",
                "Rosie",
                "Mya"
            };
            ViewBag.FatherDogIDs = FatherDogIDs;
            ViewBag.MotherDogIDs = MotherDogIDs;
        }

        // GET: LitterController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            populateDropDowns();
            return View();
        }

        // POST: LitterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Litter litter, IFormFile NewImage)
        {
            populateDropDowns();
            try
            {
                // If both the new image and old image are null the user has to add a new one
                if (NewImage == null || NewImage.Length == 0)
                {
                    if (string.IsNullOrEmpty(litter.Image))
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

                if (_litManager.GetAllLitters().Any(existingLitter => litter.LitterID == existingLitter.LitterID))
                {
                    ModelState.AddModelError("LitterID", "A litter with that ID already exists.");
                }
                if (litter.NumberPuppies > 8)
                {
                    ModelState.AddModelError("NumberPuppies", "There cannot be more than 8 puppies in a litter.");
                }
                if (litter.NumberPuppies < 1)
                {
                    ModelState.AddModelError("NumberPuppies", "There cannot be less than 1 puppy in a litter.");
                }

                if (litter.DateOfBirth >  DateTime.Now)
                {
                    ModelState.AddModelError("DateOfBirth", "The date of birth cannot be in the future.");
                }

                if (litter.DateOfBirth <= new DateTime(2020, 1, 1))
                {
                    ModelState.AddModelError("DateOfBirth", "The date of birth cannot be longer ago than 2020.");
                }
                if (litter.GoHomeDate <= new DateTime(2020, 1, 1))
                {
                    ModelState.AddModelError("GoHomeDate", "The go home date cannot be longer ago than 2020.");
                }
                if (litter.GoHomeDate < litter.DateOfBirth)
                {
                    ModelState.AddModelError("GoHomeDate", "The go home date cannot be before the date of birth.");
                }

                if (NewImage != null && NewImage.Length > 0)
                {
                    string fileExtension = Path.GetExtension(NewImage.FileName); // gets the file extension
                    string newFileName = $"{litter.LitterID}{fileExtension}"; // creates new file name

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", newFileName);

                    // creates the image in the desired location and overwrites it if it already exists
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        NewImage.CopyTo(stream);
                    }
                    litter.Image = newFileName;
                }
                if (!string.IsNullOrEmpty(litter.Image))
                {
                    ModelState.Remove("NewImage");
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        _litManager.InsertLitter(litter.LitterID, litter.FatherDogID, litter.MotherDogID, litter.Image, litter.DateOfBirth, litter.GoHomeDate, litter.NumberPuppies);
                        TempData["SuccessMessage"] = "Litter created successfully!"; // shows the message in index
                        return RedirectToAction(nameof(IndexBasic));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Image", "An error occurred please try again.");
                        return View(litter);
                    }
                }
                else
                {
                    return View(litter); // server
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: LitterController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            populateDropDowns();
            var litter = _litManager.SelectLitterByLitterID(id);
            return View(litter);
        }

        // POST: LitterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Litter litter, IFormFile NewImage)
        {
            populateDropDowns();
            try
            {
                if (litter.NumberPuppies > 8)
                {
                    ModelState.AddModelError("NumberPuppies", "There cannot be more than 8 puppies in a litter.");
                }
                if (litter.NumberPuppies < 1)
                {
                    ModelState.AddModelError("NumberPuppies", "There cannot be less than 1 puppy in a litter.");
                }
                if (litter.DateOfBirth > DateTime.Now)
                {
                    ModelState.AddModelError("DateOfBirth", "The date of birth cannot be in the future.");
                }

                if (litter.DateOfBirth <= new DateTime(2020, 1, 1))
                {
                    ModelState.AddModelError("DateOfBirth", "The date of birth cannot be longer ago than 2020.");
                }
                if (litter.GoHomeDate <= new DateTime(2020, 1, 1))
                {
                    ModelState.AddModelError("GoHomeDate", "The go home date cannot be longer ago than 2020.");
                }
                if (litter.GoHomeDate < litter.DateOfBirth)
                {
                    ModelState.AddModelError("GoHomeDate", "The go home date cannot be before the date of birth.");
                }

                // If both the new image and old image are null the user has to add a new one
                if (NewImage == null || NewImage.Length == 0)
                {
                    if (string.IsNullOrEmpty(litter.Image))
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
                    string newFileName = $"{litter.LitterID}{fileExtension}"; // creates new file name

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", newFileName);

                    // creates the image in the desired location and overwrites it if it already exists
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        NewImage.CopyTo(stream);
                    }
                    litter.Image = newFileName;
                }
                else
                {
                    ModelState.Remove("NewImage"); // If there is no new image remove it from the model state
                }


                if (ModelState.IsValid) 
                {
                    try
                    {
                        _litManager.UpdateLitter(litter.LitterID, litter.FatherDogID, litter.MotherDogID, litter.Image, litter.DateOfBirth, litter.GoHomeDate, litter.NumberPuppies);
                        TempData["SuccessMessage"] = "Litter updated successfully!"; // shows the message in index
                        return RedirectToAction(nameof(IndexBasic));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Image", "An error occurred please try again.");
                        return View(litter);
                    }
                }
                else
                {
                    return View(litter);
                }
            }
            catch
            {
                return View(litter);
            }
        }

        // GET: LitterController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string litterID, string fatherDogID, string motherDogID)
        {
            var litter = _litManager.SelectLitterByLitterID(litterID);
            var mother = _motherManager.SelectMotherDogByMotherDogID(motherDogID);
            var father = _fatherManager.SelectFatherDogByFatherDogID(fatherDogID);

            var model = new LitterDetailsViewModel
            {
                Litter = litter,
                MotherDog = mother,
                FatherDog = father,
            };

            return View(model);
        }

        // POST: LitterController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string litterID, string fatherDogID, string motherDogID, Litter litter)
        {
            Litter _litter = _litManager.SelectLitterByLitterID(litterID);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", _litter.Image); // gets the image path so it can be deleted
            try
            {
                int result = _litManager.DeleteLitterByLitterID(litterID);

                if (result > 0)
                {
                    System.IO.File.Delete(imagePath); //delete the litter image
                    TempData["SuccessMessage"] = "Litter deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Delete failed.";
                }
                return RedirectToAction(nameof(IndexBasic));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View();
            }
        }
    }
}
