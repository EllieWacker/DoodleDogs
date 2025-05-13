using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IPuppyAccessor
    {
        int DeletePuppyByPuppyID(string PuppyID);
        int UpdateFullPuppy(string puppyID, string breedID, string litterID, string medicalRecordID, string image, string gender, bool adopted, bool microchip, decimal price);
        int InsertPuppy(string puppyID, string breedID, string litterID, string medicalRecordID, string image, string gender, bool adopted, bool microchip, decimal price);
        Puppy SelectPuppyByPuppyID(string puppyID);
        List<Puppy> SelectPuppiesByLitterID(string litterID);
        List<Puppy> SelectPuppiesByBreedID(string breedID);
        int UpdatePuppy(string puppyID, bool oldAdopted, bool newAdopted);
        List<Puppy> SelectAllPuppies();
    }
}
