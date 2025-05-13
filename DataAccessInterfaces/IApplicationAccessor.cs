using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IApplicationAccessor
    {
        List<Application> SelectApplicationsByUserID(int userID);
        int InsertApplication(int userID, string fullName, int age, bool renting, bool yard, string desiredBreed, string desiredGender, string preferredContact, bool status, string comment);

        int UpdateApplicationStatus(int applicationID, bool oldStatus, bool newStatus);
        List<Application> SelectAllApplications();
    }
}
