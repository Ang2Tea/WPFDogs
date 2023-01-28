using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogAPI.Core.Models
{
    public class DogBreed
    {
        public string Breed { get; set; }
        public string BreedName { get; set; }
        public string ImagesURL { get; set; }

        public DogBreed(string breed)
        {
            Breed = breed;
            BreedName = breed;
            ImagesURL = $"https://dog.ceo/api/breed/{Breed}/images";
        }
    }
    public class SubDogBreed : DogBreed
    {
        public string SubBreed { get; set; }

        public SubDogBreed(string breed, string subBreed) : base(breed)
        {
            SubBreed = subBreed;
            BreedName = Breed + " " + SubBreed;
            ImagesURL = $"https://dog.ceo/api/breed/{Breed}/{SubBreed}/images";
        }
    }
}
