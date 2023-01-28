using DogAPI.Core.Models;
using DogAPI.Core.Util;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DogAPI.Core.ViewModels;

internal class MainWindowViewModel : BaseViewModel
{
    private readonly DogsAPI dogsAPI = new();

    private ObservableCollection<string> images = new();
    private DogBreed? selectedDog;
    private string? searchText;

    public List<DogBreed> Dogs { get; } = new();
    public ObservableCollection<string> Images
    {
        get => images;
        set => Set(ref images, value);
    }
    public DogBreed? SelectedDog
    {
        get => selectedDog;
        set
        {
            Set(ref selectedDog, value);

            if (selectedDog is null) return;

            Task.Run(() => Images = new(DogsAPI.StaticPost<List<string>>(selectedDog.ImagesURL)));
        }
    }
    public string? SearchText
    {
        get => searchText;
        set
        {
            Set(ref searchText, value);

            if (searchText is null) return;

            if (searchText == "")
            {
                Task.Run(() => images = new(dogsAPI.Post<List<string>>("/breeds/image/random/30")));
                return;
            }
            Task.Run(() =>
            {
                Regex reg = new($"\\w*{searchText}\\w*");

                List<string> list = new();

                var isMatch = Dogs.Where(p => reg.IsMatch(p.BreedName));
                foreach (DogBreed item in isMatch) { list.AddRange(DogsAPI.StaticPost<List<string>>(item.ImagesURL)); }

                Images = new(list);
            });
        }
    }

    public MainWindowViewModel()
    {
        _ = Parallel.ForEach(
                    dogsAPI.Post<Dictionary<string, string[]>>("/breeds/list/all"),
                    CreateDogs
                    );
        images = new(dogsAPI.Post<List<string>>("/breeds/image/random/30"));
    }

    private void CreateDogs(KeyValuePair<string, string[]> item)
    {
        if (item.Value.Length == 0)
        {
            DogBreed newDog = new(item.Key);
            Dogs.Add(newDog);
            return;
        }

        for (int i = 0; i < item.Value.Length; i++)
        {
            DogBreed newDog = new SubDogBreed(item.Key, item.Value[i]);
            Dogs.Add(newDog);
        }
    }

}