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
    public class ApplicationManager : IApplicationManager
    {
        private IApplicationAccessor _applicationAccessor;

        public ApplicationManager()
        {
            _applicationAccessor = new ApplicationAccessor();

        }

        public ApplicationManager(IApplicationAccessor applicationAccessor)
        {
            _applicationAccessor = applicationAccessor;

        }
        public List<Application> SelectApplicationsByUserID(int userID)
        {
            List<Application> applications = new List<Application>();
            try
            {
                applications = _applicationAccessor.SelectApplicationsByUserID(userID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Application not found", ex);
            }
            return applications;
        }

        public int InsertApplication(int userID, string fullName, int age, bool renting, bool yard, string desiredBreed, string desiredGender, string preferredContact, bool status, string comment)
        {
            int result = 0;
            try
            {
                result = _applicationAccessor.InsertApplication(userID, fullName, age, renting, yard, desiredBreed, desiredGender, preferredContact, status, comment);
                if (result == 0)
                {
                    throw new ApplicationException("Insert Application Failed");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Insert failed." + ex.Message, ex);
            }


            return result;
        }
        public List<Application> GetAllApplications()
        {
            List<Application> applications = null;

            try
            {
                applications = _applicationAccessor.SelectAllApplications();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data not available", ex);
            }

            return applications;
        }
        public int UpdateApplicationStatus(int applicationID, bool oldStatus, bool newStatus)
        {
            int result = 0;
            try
            {
                result = _applicationAccessor.UpdateApplicationStatus(applicationID, oldStatus, newStatus);
                if (result == 0)
                {
                    throw new ApplicationException("Application not updated");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed.", ex);
            }

            return result;
        }

    }
}
