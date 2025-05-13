using DataAccessInterfaces;
using DataDomain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessFakes
{
    public class AdoptionAccessorFake : IAdoptionAccessor
    {
        private List<Adoption> _adoptions;
        public AdoptionAccessorFake()
        {
            _adoptions = new List<Adoption>();
            _adoptions.Add(new Adoption() { AdoptionID= 1, ApplicationID = 1, PuppyID="Aussie", UserID = 1, Status="Too young" });
            _adoptions.Add(new Adoption() { AdoptionID = 2, ApplicationID = 2, PuppyID = "Aussie", UserID = 2, Status="Almost ready" });
        }
        public int InsertAdoption(int applicationID, string puppyID, int userID, string status)
        {
            int result = 0;
            var _adoption = new Adoption()
            {
                AdoptionID = _adoptions.Count -1,
                ApplicationID = applicationID,
                PuppyID = puppyID,
                UserID = userID,
                Status = status
            };

            _adoptions.Add(_adoption);
            result = _adoption.AdoptionID;

            if (result == 0)
            {
                throw new ArgumentException("Unable to insert adoption");
            }
            return result;
        }

        public List<Adoption> SelectAllAdoptions()
        {

            if (_adoptions.Count == 0)
            {
                throw new ArgumentException("Adoption record not found");
            }

            return _adoptions;
        }

        public int UpdateAdoption(int adoptionID, string oldStatus, string newStatus)
        {
            int count = 0;
            for (int i = 0; i < _adoptions.Count(); i++)
            {
                if (_adoptions[i].AdoptionID == adoptionID)
                {
                    _adoptions[i].Status = newStatus;
                    count++;
                }
            }
            if (count == 0)
            {
                throw new ArgumentException("Adoption record not found");
            }
            return count;
        }

    }
}
