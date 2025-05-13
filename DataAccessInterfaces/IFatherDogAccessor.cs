using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IFatherDogAccessor
    {
        List<FatherDog> SelectAllFatherDogs();

        FatherDog SelectFatherDogByFatherDogID(string fatherDogID);
    }
}
