using DataAccessInterfaces;
using DataAccessLayer;
using DataDomain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class AdoptionManager : IAdoptionManager
    {
        private IAdoptionAccessor _adoptionAccessor;

        public AdoptionManager()
        {
            _adoptionAccessor = new AdoptionAccessor();

        }

        public AdoptionManager(IAdoptionAccessor adoptionAccessor)
        {
            _adoptionAccessor = adoptionAccessor;

        }
        public int InsertAdoption(int applicationID, string puppyID, int userID, string status)
        {
            int result = 0;
            try
            {
                result = _adoptionAccessor.InsertAdoption(applicationID, puppyID, userID, status);
                if (result == 0)
                {
                    throw new ApplicationException("Insert Adoption Failed");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Insert failed." + ex.Message, ex);
            }


            return result;
        }
        public List<Adoption> GetAllAdoptions()
        {
            List<Adoption> adoptions = null;

            try
            {
                adoptions = _adoptionAccessor.SelectAllAdoptions();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not available", ex);
            }

            return adoptions;
        }

        public int UpdateAdoption(int adoptionID, string oldStatus, string newStatus)
        {
            int result = 0;
            try
            {
                result = _adoptionAccessor.UpdateAdoption(adoptionID, oldStatus, newStatus);
                if (result == 0)
                {
                    throw new ApplicationException("Status not updated");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed", ex);
            }

            return result;
        }
    }
}
