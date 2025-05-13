using DataDomain;

namespace DogBreederMVCApp.Models
{
    public class PuppyAdoptionViewModel
    {
        public Puppy Puppy { get; set; }
        public Adoption Adoption { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
    }
}
