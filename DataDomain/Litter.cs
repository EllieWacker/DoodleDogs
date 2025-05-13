using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomain
{
    public class Litter
    {
        [Required(ErrorMessage = "Litter is required.")]
        public string? LitterID { get; set; }
        [Required(ErrorMessage = "Father Dog is required.")]
        public string? FatherDogID { get; set; }
        [Required(ErrorMessage = "Mother Dog is required.")]
        public string? MotherDogID { get; set; }
        
        public string? Image { get; set; }
        [Required(ErrorMessage = "Date Of Birth is required.")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Go Home Date is required.")]
        public DateTime GoHomeDate { get; set; }
        [Required(ErrorMessage = "Number of Puppies is required.")]
        public int NumberPuppies { get; set; }
    }
}
