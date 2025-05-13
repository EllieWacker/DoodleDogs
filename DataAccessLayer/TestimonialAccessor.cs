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
    public class TestimonialAccessor : ITestimonialAccessor
    {
        public Testimonial SelectTestimonialByTestimonialID(string testimonialID)
        {
            Testimonial testimonial = null;

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_testimonial_by_testimonialID", conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            //parameters
            cmd.Parameters.Add("@TestimonialID", SqlDbType.NChar);

            //values
            cmd.Parameters["@TestimonialID"].Value = testimonialID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    testimonial = new Testimonial()
                    {
                        TestimonialID = reader.GetString(0),
                        AdoptionID = reader.GetInt32(1),
                        Image = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Details = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Rating = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                    };
                }
                else
                {
                    throw new ArgumentException("Testimonial record not found");
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

            return testimonial;
        }

        public List<Testimonial> SelectAllTestimonials()
        {
            List<Testimonial> testimonials = new List<Testimonial>();

            // connection
            var conn = DBConnection.GetConnection();

            //command
            var cmd = new SqlCommand("sp_select_all_testimonials", conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var testimonial = new Testimonial()
                        {
                            TestimonialID = reader.GetString(0),
                            AdoptionID = reader.GetInt32(1),
                            Image = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Details = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Rating = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                        };
                        testimonials.Add(testimonial);
                    }
                }
                else
                {
                    throw new ArgumentException("Testimonial list not found");
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

            return testimonials;
        }

    }



}
