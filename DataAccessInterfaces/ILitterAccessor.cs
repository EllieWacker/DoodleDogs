using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface ILitterAccessor
    {
        int DeleteLitterByLitterID(string litterID);
        int UpdateLitter(string litterID, string fatherDogID, string motherDogID, string image, DateTime dateOfBirth, DateTime goHomeDate, int numberPuppies);
        int InsertLitter(string litterID, string fatherDogID, string motherDogID, string image, DateTime dateOfBirth, DateTime goHomeDate, int numberPuppies);
        Litter SelectLitterByLitterID(string litterID);
        List<Litter> SelectAllLitters();
    }
}
