using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IApplicationManager
    {
        public List<Application> SelectApplicationsByUserID(int userID);
        public int InsertApplication(int userID, string fullName, int age, bool renting, bool yard, string desiredBreed, string desiredGender, string preferredContact, bool status, string comment);
        public List<Application> GetAllApplications();

        public int UpdateApplicationStatus(int applicationID, bool oldStatus, bool newStatus);

    }
}
