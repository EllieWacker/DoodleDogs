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
    public class FatherDogAccessorFake : IFatherDogAccessor
    {
        private List<FatherDog> _fatherDogs;
        public FatherDogAccessorFake()
        {
            _fatherDogs = new List<FatherDog>();
            _fatherDogs.Add(new FatherDog() { FatherDogID = "Fonzi", BreedID = "Aussiedoodle", Personality="Hyper", EnergyLevel="high", BarkingLevel = "low", Trainability="Really good", Image="cockapoo.jpg", Description="He's great" });
            _fatherDogs.Add(new FatherDog() { FatherDogID = "Carter", BreedID = "mini poodle", Personality = "calm", EnergyLevel = "high", BarkingLevel = "high", Trainability = "Good", Image = "aussiedoodle.jpg", Description = "He's a really sweet boy!" });

        }
        public List<FatherDog> SelectAllFatherDogs()
        {
            return _fatherDogs;
        }
        public FatherDog SelectFatherDogByFatherDogID(string fatherDogID)
        {
            foreach (var fatherDog in _fatherDogs)
            {
                if (fatherDog.FatherDogID == fatherDogID)
                {
                    return fatherDog;
                }
            }
            throw new ArgumentException("FatherDog record not found");
        }


    }
}
