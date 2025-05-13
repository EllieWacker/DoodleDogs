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
    public class PuppyManager : IPuppyManager
    {
        private IPuppyAccessor _puppyAccessor;

        public PuppyManager()
        {
            _puppyAccessor = new PuppyAccessor();

        }

        public PuppyManager(IPuppyAccessor puppyAccessor)
        {
            _puppyAccessor = puppyAccessor;

        }


        public bool UpdateEntirePuppy(string puppyID, string breedID, string litterID, string medicalRecordID, string image, string gender, bool adopted, bool microchip, decimal price)
        {
            bool result = false;

            try
            {
                result = (1 == _puppyAccessor.UpdateFullPuppy(puppyID, breedID, litterID, medicalRecordID, image, gender, adopted, microchip, price));

                if (!result)
                {
                    throw new ApplicationException("No Puppy Record Found");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed.", ex);
            }

            return result;  
        }


        public int InsertPuppy(string puppyID, string breedID, string litterID, string medicalRecordID, string image, string gender, bool adopted, bool microchip, decimal price)
        {
            int result = 0;
            try
            {
                result = (_puppyAccessor.InsertPuppy( puppyID,  breedID,  litterID,  medicalRecordID,  image,  gender,  adopted,  microchip,  price));
                if (result == 0)
                {
                    Console.WriteLine($"Insert failed for PuppyID: {puppyID}, result: {result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during insert: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw new ApplicationException("Insert Puppy Failed in the catch", ex);
            }
            return result;
        }


        public List<Puppy> SelectPuppiesByBreedID(string breedID)
        {
            List<Puppy> puppies = new List<Puppy>();
            try
            {
                puppies = _puppyAccessor.SelectPuppiesByBreedID(breedID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Puppy list not found", ex);
            }
            return puppies;
        }

        public List<Puppy> SelectPuppiesByLitterID(string litterID)
        {
            List<Puppy> puppies = new List<Puppy>();
            try
            {
                puppies = _puppyAccessor.SelectPuppiesByLitterID(litterID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Puppy list not found", ex);
            }
            return puppies;
        }

        public int UpdatePuppy(string puppyID, bool oldApprovedPuppy, bool newApprovedPuppy)
        {
            int result = 0;
            try
            {
                result = _puppyAccessor.UpdatePuppy(puppyID, oldApprovedPuppy, newApprovedPuppy);
                if (result == 0)
                {
                    throw new ApplicationException("Puppy not updated");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed", ex);
            }

            return result;
        }

        public List<Puppy> GetAllPuppies()
        {
            List<Puppy> puppies = new List<Puppy>();
            try
            {
                puppies = _puppyAccessor.SelectAllPuppies();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Puppy list not found", ex);
            }
            return puppies;
        }

        public Puppy GetPuppyByPuppyID(string puppyID)
        {
            Puppy puppy = null;
            try
            {
                puppy = _puppyAccessor.SelectPuppyByPuppyID(puppyID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Puppy not found", ex);
            }
            return puppy;
        }

        public int DeletePuppyByPuppyID(string puppyID)
        {
            int result = 0;
            try
            {
                result = _puppyAccessor.DeletePuppyByPuppyID(puppyID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Delete Puppy Failed", ex);
            }
            return result;
        }
    }
}
