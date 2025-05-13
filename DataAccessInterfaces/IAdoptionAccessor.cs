using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IAdoptionAccessor
    {
        int InsertAdoption(int applicationID, string puppyID, int userID, string status);
        List<Adoption> SelectAllAdoptions();
        int UpdateAdoption(int adoptionID, string oldStatus, string newStatus);
    }
}
