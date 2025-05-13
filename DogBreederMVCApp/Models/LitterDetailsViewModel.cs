using DataDomain;

namespace DogBreederMVCApp.Models
{
    public class LitterDetailsViewModel
    {
        public Litter Litter { get; set; }
        public MotherDog MotherDog { get; set; }
        public FatherDog FatherDog { get; set; }   
    }
}
