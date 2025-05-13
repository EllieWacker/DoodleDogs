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
    public class LitterAccessorFake : ILitterAccessor
    {
        private List<Litter> _litters;
        public LitterAccessorFake()
        {
            _litters = new List<Litter>();
            _litters.Add(new Litter() { LitterID = "AussieLit5", FatherDogID="Harold", MotherDogID="Clemmy", Image = "lit5.jpg", DateOfBirth= new DateTime(2004, 12, 5), GoHomeDate = new DateTime(2005,2, 5), NumberPuppies = 2 });
            _litters.Add(new Litter() { LitterID = "AussieLit6", FatherDogID = "Harold", MotherDogID = "Mya", Image = "lit6.jpg", DateOfBirth = new DateTime(2004, 12, 5), GoHomeDate = new DateTime(2005, 2, 5) });
        }

        public List<Litter> SelectAllLitters()
        {
            List<Litter> litters = new List<Litter>();
            foreach (var litter in _litters)
            {
                litters.Add(litter);
            }
            return litters;
        }

        public Litter SelectLitterByLitterID(string litterID)
        {
            foreach (var litter in _litters)
            {
                if (litter.LitterID == litterID)
                {
                    return litter;
                }
            }
            throw new ArgumentException("Litter record not found");
        }

        public int UpdateLitter(string litterID, string fatherDogID, string motherDogID, string image, DateTime dateOfBirth, DateTime goHomeDate, int numberPuppies)
        {
            int count = 0;
            for (int i = 0; i < _litters.Count(); i++)
            {

                if (_litters[i].LitterID == litterID)
                {
                    _litters[i].FatherDogID = fatherDogID;
                    _litters[i].MotherDogID = motherDogID;
                    _litters[i].Image = image;
                    _litters[i].DateOfBirth = dateOfBirth;
                    _litters[i].GoHomeDate = goHomeDate;
                    _litters[i].NumberPuppies = numberPuppies;
                    count++;
                }
            }
            if (count == 0)
            {
                throw new ArgumentException("Litter record not found");
            }
            return count;
        }

        public int InsertLitter(string litterID, string fatherDogID, string motherDogID, string image, DateTime dateOfBirth, DateTime goHomeDate, int numberPuppies)
        {
            int result = 0;
            var _litter = new Litter()
            {
                LitterID = litterID,
                FatherDogID = fatherDogID,
                MotherDogID = motherDogID,
                Image = image,
                GoHomeDate = goHomeDate,
                DateOfBirth = dateOfBirth,
                NumberPuppies = numberPuppies
            };
            _litters.Add(_litter);
            result = numberPuppies;
            if (result == 0)
            {
                throw new ArgumentException("Unable to insert litter");
            }
            return result;
        }

        public int DeleteLitterByLitterID(string litterID)
        {
            int count = 0;
            foreach (var litter in _litters)
            {
                if (litter.LitterID == litterID)
                {
                    litter.LitterID = "0";
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
