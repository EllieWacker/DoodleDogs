using DataAccessInterfaces;
using DataDomain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace DataAccessFakes
{
    public class PuppyAccessorFake : IPuppyAccessor
    {
        private List<Puppy> _puppies;
        public PuppyAccessorFake()
        {
            _puppies = new List<Puppy>();
            _puppies.Add(new Puppy() { PuppyID = "ALit1One", BreedID = "Aussiedoodle", LitterID="AussieLit1", MedicalRecordID="Luke1", Image = "fonzi.jpg", Gender = "Female", Adopted = false, Microchip = true, Price= 800.00m});
            _puppies.Add(new Puppy() { PuppyID = "CLit1One", BreedID = "Cockapoo", LitterID = "CockerLit1", MedicalRecordID = "Luke2", Image = "fonzi.jpg", Gender = "Male", Adopted = false, Microchip = true, Price = 900.00m });
        }


        public Puppy SelectPuppyByPuppyID(string puppyID)
        {
            foreach (var puppy in _puppies)
            {
                if (puppy.PuppyID == puppyID)
                {
                    return puppy;
                }
            }
            throw new ArgumentException("Puppy record not found");
        }
        public List<Puppy> SelectAllPuppies()
        {
            List<Puppy> puppies = new List<Puppy>();
            foreach (var puppy in _puppies)
            {
                puppies.Add(puppy);
            }
            return puppies;
        }

        public List<Puppy> SelectPuppiesByBreedID(string breedID)
        {
            List<Puppy> litterPuppies = new List<Puppy>();
            foreach (var puppy in _puppies)
            {
                if (puppy.BreedID == breedID)
                {
                    litterPuppies.Add(puppy);
                }
            }
            if (breedID.Length == 0)
            {
                throw new ArgumentException("Puppy record not found");
            }
            return litterPuppies;
        }

        public List<Puppy> SelectPuppiesByLitterID(string litterID)
        {
            List<Puppy> litterPuppies = new List<Puppy>();
            foreach (var puppy in _puppies)
            {
                if (puppy.LitterID == litterID)
                {
                    litterPuppies.Add(puppy);
                }
            }
            if (litterID.Length == 0)
            {
                throw new ArgumentException("Puppy record not found");
            }
            return litterPuppies;
        }

        public int UpdatePuppy(string puppyID, bool oldAdopted, bool newAdopted)
        {
            int count = 0;
            for (int i = 0; i < _puppies.Count(); i++)
            {
                if (_puppies[i].PuppyID == puppyID)
                {
                    _puppies[i].Adopted = newAdopted;
                    count++;
                }
            }
            if (count == 0)
            {
                throw new ArgumentException("Puppy record not found");
            }
            return count;
        }

        public int UpdateFullPuppy(string puppyID, string breedID, string litterID, string medicalRecordID, string image, string gender, bool adopted, bool microchip, decimal price)
        {
            int count = 0;
            for (int i = 0; i < _puppies.Count(); i++)
            {
              
                if (_puppies[i].PuppyID == puppyID)
                {
                    _puppies[i].BreedID = breedID;
                    _puppies[i].LitterID = litterID;
                    _puppies[i].MedicalRecordID = medicalRecordID;
                    _puppies[i].Image = image;
                    _puppies[i].Gender = gender;
                    _puppies[i].Adopted = adopted;
                    _puppies[i].Microchip = microchip;
                    _puppies[i].Price = price;

                    count++; 
                }
            }
            if (count == 0)
            {
                throw new ArgumentException("Puppy record not found");
            }
            return count; 
        }

        public int InsertPuppy(string puppyID, string breedID, string litterID, string medicalRecordID, string image, string gender, bool adopted, bool microchip, decimal price)
        {
            int result = 0;
            var _puppy = new Puppy()
            {
                PuppyID = puppyID,
                BreedID = breedID,
                LitterID = litterID,
                MedicalRecordID = medicalRecordID,
                Image = image,
                Gender = gender,
                Adopted = adopted,
                Microchip = microchip,
                Price = price
            };
            _puppies.Add(_puppy);
            result = (int)_puppy.Price;
            if (result == 0)
            {
                throw new ArgumentException("Unable to insert puppy");
            }
            return result;
        }

        public int DeletePuppyByPuppyID(string PuppyID)
        {
            int count = 0;
            foreach (var puppy in _puppies)
            {
                if (puppy.PuppyID == PuppyID)
                {
                    puppy.PuppyID = "0";
                    count++;
                }
            }
            if (count == 0)
            {
                throw new ArgumentException("Delete Failed");
            }
            return count;
        }
    }
}
