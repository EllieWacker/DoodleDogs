using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomain
{
    public class Breed
    {
        public string? BreedID { get; set; }
        public string? Size { get; set; }
        public string? Image { get; set; }
        public bool? Hypoallergenic { get; set; }
        public string? LifeExpectancy { get; set; }
        public bool? GoodDogs { get; set; }
        public bool? GoodKids { get; set; }
        public string? Description { get; set; }
    }
}
