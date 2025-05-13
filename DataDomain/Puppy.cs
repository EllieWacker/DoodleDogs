using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomain
{
    public class Puppy
    {
        [Required(ErrorMessage = "Puppy Name is required.")]
        public string? PuppyID { get; set; }
        [Required(ErrorMessage = "Breed is required.")]
        public string? BreedID { get; set; }
        [Required(ErrorMessage = "Litter is required.")]
        public string? LitterID { get; set; }
        [Required(ErrorMessage = "Medical Record is required.")]
        public string? MedicalRecordID { get; set; }
        public string? Image {  get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }
        [Required(ErrorMessage = "Adopted is required.")]
        public bool Adopted { get; set; }
        [Required(ErrorMessage = "Microchip is required.")]
        public bool Microchip { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public decimal? Price { get; set; }
    }
}
