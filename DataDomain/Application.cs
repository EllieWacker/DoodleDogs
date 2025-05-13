using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomain
{
    public class Application
    {
        public int ApplicationID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required(ErrorMessage = "Full name is required.")]
        public string? FullName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public bool Renting { get; set; }
        [Required]
        public bool Yard { get; set; }
        [Required]
        public string? DesiredBreed { get; set; }
        [Required]
        public string? DesiredGender { get; set; }
        [Required]
        public string? PreferredContact { get; set; }
        public bool Status { get; set; }
        public string? Comment { get; set; }
    }
}
