using DataAccessInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Data;
using System.Data;
using DataDomain;

namespace DataAccessLayer
{
    public class BreedAccessor : IBreedAccessor
    {
        public Breed SelectBreedByBreedID(string breedID)
        {
            Breed breed = null;

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_breed_by_breedID", conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            //parameters
            cmd.Parameters.Add("@BreedID", SqlDbType.NVarChar, 50);

            //values
            cmd.Parameters["@BreedID"].Value = breedID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    breed = new Breed()
                    {
                        BreedID = reader.GetString(0),
                        Size = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Image = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Hypoallergenic = reader.IsDBNull(3) ? null : reader.GetBoolean(3),
                        LifeExpectancy = reader.GetString(4),
                        GoodDogs = reader.IsDBNull(5) ? null : reader.GetBoolean(5),
                        GoodKids = reader.IsDBNull(6) ? null : reader.GetBoolean(6),
                        Description = reader.GetString(7),
                    };
                }
                else
                {
                    throw new ArgumentException("Breed record not found");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return breed;
        }

    }


    
   }
