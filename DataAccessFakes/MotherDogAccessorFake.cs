using DataAccessInterfaces;
using DataDomain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class MotherDogAccessorFake : IMotherDogAccessor
    {
        private List<MotherDog> _motherDogs;
        public MotherDogAccessorFake()
        {
            _motherDogs = new List<MotherDog>();
            _motherDogs.Add(new MotherDog() { MotherDogID = "Gracie", BreedID = "Aussiedoodle", Personality = "Hyper", EnergyLevel = "high", BarkingLevel = "low", Trainability = "Really good", Image = "gracie.jpg", Description = "He's great" });
            _motherDogs.Add(new MotherDog() { MotherDogID = "Coco", BreedID = "mini poodle", Personality = "calm", EnergyLevel = "high", BarkingLevel = "high", Trainability = "Good", Image = "aussiedoodle.jpg", Description = "He's a really sweet boy!" });

        }

        public List<MotherDog> SelectAllMotherDogs()
        {
            return _motherDogs;
        }
        public MotherDog SelectMotherDogByMotherDogID(string motherDogID)
        {
            foreach (var motherDog in _motherDogs)
            {
                if (motherDog.MotherDogID == motherDogID)
                {
                    return motherDog;
                }
            }
            throw new ArgumentException("MotherDog record not found");
        }


    }
}
