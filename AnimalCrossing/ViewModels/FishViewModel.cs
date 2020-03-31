using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AnimalCrossing.Models;
using AnimalCrossing.Services;
using GalaSoft.MvvmLight;

namespace AnimalCrossing.ViewModels
{
    public class FishViewModel : ViewModelBase
    {
        public ObservableCollection<Fish> Fishes { get; private set; } = new ObservableCollection<Fish>();

        public FishViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Fishes.Clear();
            using (var con = SQLiteService.GetDbConnection())
            {
                var animals = con.Table<AnimalsFish>().ToList();
                foreach (var item in animals)
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Fish>(item.Data);
                    Fishes.Add(obj);
                }
            }
        }
    }
}
