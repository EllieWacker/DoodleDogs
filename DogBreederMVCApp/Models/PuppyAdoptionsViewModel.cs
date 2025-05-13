using DataDomain;

namespace DogBreederMVCApp.Models
{
    public class PuppyAdoptionsViewModel
    {
        public List<Puppy> Puppies { get; set; }
        public List<Adoption> Adoptions { get; set; }
        public List<Litter> Litters { get; set; }
    }
}
