using DataAccessInterfaces;
using DataAccessLayer;
using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class LitterManager : ILitterManager
    {
        private ILitterAccessor _litterAccessor;

        public LitterManager()
        {
            _litterAccessor = new LitterAccessor();

        }

        public LitterManager(ILitterAccessor litterAccessor)
        {
            _litterAccessor = litterAccessor;

        }
        public Litter SelectLitterByLitterID(string litterID)
        {
            Litter litter = null;
            try
            {
                litter = _litterAccessor.SelectLitterByLitterID(litterID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Litter not found", ex);
            }
            return litter;
        }

        public List<Litter> GetAllLitters()
        {
            List<Litter> litters = new List<Litter>();
            try
            {
                litters = _litterAccessor.SelectAllLitters();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Litter list not found", ex);
            }
            return litters;
        }

        public bool UpdateLitter(string litterID, string fatherDogID, string motherDogID, string image, DateTime dateOfBirth, DateTime goHomeDate, int numberPuppies)
        {
            bool result = false;

            try
            {
                result = (1 == _litterAccessor.UpdateLitter(litterID, fatherDogID, motherDogID, image, dateOfBirth, goHomeDate, numberPuppies));

                if (!result)
                {
                    throw new ApplicationException("No Litter Record Found");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed.", ex);
            }

            return result;
        }


        public int InsertLitter(string litterID, string fatherDogID, string motherDogID, string image, DateTime dateOfBirth, DateTime goHomeDate, int numberPuppies)
        {
            int result = 0;
            try
            {
                result = (_litterAccessor.InsertLitter(litterID, fatherDogID, motherDogID, image, dateOfBirth, goHomeDate, numberPuppies));
                if (result == 0)
                {
                    Console.WriteLine($"Insert failed for LitterID: {litterID}, result: {result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new ApplicationException("Insert Litter Failed in the catch", ex);
            }
            return result;
        }

        public int DeleteLitterByLitterID(string litterID)
        {
            int result = 0;
            try
            {
                result = _litterAccessor.DeleteLitterByLitterID(litterID);
            }
            catch (Exception ex) 
            {
                throw new ApplicationException("Delete Litter Failed", ex);
            }
            return result;
        }
    }
}
