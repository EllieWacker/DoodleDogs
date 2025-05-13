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
    public class BreedAccessorFake : IBreedAccessor
    {
        private List<Breed> _breeds;
        public BreedAccessorFake()
        {
            _breeds = new List<Breed>();
            _breeds.Add(new Breed() {BreedID = "German Shepard", Size="12-14 inches", Image="fonzi.jpg", Hypoallergenic=false, GoodDogs=false, GoodKids=true,Description = "really cool"});
            _breeds.Add(new Breed() { BreedID = "Poodle", Size = "9-10 inches", Image = "luke.jpg", Hypoallergenic = true, GoodDogs = true, GoodKids = true, Description = "Nice Dog" });
        }
        public Breed SelectBreedByBreedID(string breedID)
        {
            foreach (var breed in _breeds)
            {
                if(breed.BreedID == breedID)
                {
                    return breed;
                }
            }
            throw new ArgumentException("Breed record not found");
        }


    }
}
