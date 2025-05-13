using DataAccessInterfaces;
using DataAccessLayer;
using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class MotherDogManager : IMotherDogManager
    {
        private IMotherDogAccessor _motherDogAccessor;

        public MotherDogManager()
        {
            _motherDogAccessor = new MotherDogAccessor();

        }

        public MotherDogManager(IMotherDogAccessor motherDogAccessor)
        {
            _motherDogAccessor = motherDogAccessor;

        }

        public List<MotherDog> GetAllMotherDogs()
        {
            List<MotherDog> motherDogs = new List<MotherDog>();
            try
            {
                motherDogs = _motherDogAccessor.SelectAllMotherDogs();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("MotherDog list not found", ex);
            }
            return motherDogs;
        }
        public MotherDog SelectMotherDogByMotherDogID(string motherDogID)
        {
            MotherDog motherDog = null;
            try
            {
                motherDog = _motherDogAccessor.SelectMotherDogByMotherDogID(motherDogID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("MotherDog not found", ex);
            }
            return motherDog;
        }

    }
}
