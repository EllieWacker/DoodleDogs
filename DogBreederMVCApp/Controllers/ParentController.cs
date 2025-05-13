using DataDomain;
using DogBreederMVCApp.Models;
using LogicLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing;

namespace DogBreederMVCApp.Controllers
{
    public class ParentController : Controller
    {
        private IMotherDogManager _motherManager = new MotherDogManager();
        private IFatherDogManager _fatherManager = new FatherDogManager();

        // GET: ParentController
        public ActionResult FatherIndex()
        {
            var dads = _fatherManager.GetAllFatherDogs();
            return View(dads);
        }

        // GET: ParentController
        public ActionResult MotherIndex()
        {
            var moms = _motherManager.GetAllMotherDogs();
            return View(moms);
        }

        // GET: ParentController
        [Authorize(Roles = "Admin")]
        public ActionResult BothBasicIndex()
        {
            var moms = _motherManager.GetAllMotherDogs();
            var dads = _fatherManager.GetAllFatherDogs();
            var model = new ParentsViewModel
            {
                Mothers = moms,
                Fathers = dads
            };
            return View(model);
        }
    }
}
