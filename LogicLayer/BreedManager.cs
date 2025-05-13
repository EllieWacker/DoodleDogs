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
    public class BreedManager : IBreedManager
    {
        private IBreedAccessor _breedAccessor;

        public BreedManager()
        {
            _breedAccessor = new BreedAccessor();

        }

        public BreedManager(IBreedAccessor breedAccessor)
        {
            _breedAccessor = breedAccessor;

        }
        public Breed SelectBreedByBreedID(string breedID)
        {
            Breed breed = null;
            try
            {
                breed = _breedAccessor.SelectBreedByBreedID(breedID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Breed not found", ex);
            }
            return breed;
        }

    }
}
