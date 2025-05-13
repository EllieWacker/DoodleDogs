using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IAdoptionManager
    {
        public int InsertAdoption(int applicationID, string puppyID, int userID, string status);
        public List<Adoption> GetAllAdoptions();
        public int UpdateAdoption(int adoptionID, string oldStatus, string newStatus);

    }
}
