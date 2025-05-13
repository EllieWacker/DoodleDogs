using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface ILitterManager
    {
        public bool UpdateLitter(string litterID, string fatherDogID, string motherDogID, string image, DateTime dateOfBirth, DateTime goHomeDate, int numberPuppies);
        public int InsertLitter(string litterID, string fatherDogID, string motherDogID, string image, DateTime dateOfBirth, DateTime goHomeDate, int numberPuppies);
        public int DeleteLitterByLitterID(string litterID);
        public Litter SelectLitterByLitterID(string litterID);
        public List<Litter> GetAllLitters();
    }
}
