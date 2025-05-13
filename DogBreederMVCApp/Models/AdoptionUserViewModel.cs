using DataDomain;

namespace DogBreederMVCApp.Models
{
    public class AdoptionUserViewModel
    {
        public List<Adoption> Adoptions { get; set; }
        public List<User> Users { get; set; }
    }
}
