using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IPuppyManager
    {
        public int InsertPuppy(string puppyID, string breedID, string litterID, string medicalRecordID, string image, string gender, bool adopted, bool microchip, decimal price);
        public int DeletePuppyByPuppyID(string puppyID);
        public bool UpdateEntirePuppy(string puppyID, string breedID, string litterID, string medicalRecordID, string image, string gender, bool adopted, bool microchip, decimal price);
        public Puppy GetPuppyByPuppyID(string puppyID);
        public List<Puppy> SelectPuppiesByLitterID(string litterID);
        public List<Puppy> SelectPuppiesByBreedID(string breedID);

        public int UpdatePuppy(string puppyID, bool oldApprovedPuppy, bool newApprovedPuppy);
        public List<Puppy> GetAllPuppies();
    }
}
