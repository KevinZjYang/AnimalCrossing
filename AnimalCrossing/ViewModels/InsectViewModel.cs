using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AnimalCrossing.Models;
using AnimalCrossing.Services;
using GalaSoft.MvvmLight;

namespace AnimalCrossing.ViewModels
{
    public class InsectViewModel : ViewModelBase
    {
        public ObservableCollection<Insect> Insects { get; private set; } = new ObservableCollection<Insect>();

        public InsectViewModel()
        {
        }

        public void LoadData()
        {
            Insects.Clear();
            using (var con = SQLiteService.GetDbConnection())
            {
                var animals = con.Table<AnimalsInsect>().ToList();
                foreach (var item in animals)
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Insect>(item.Data);
                    Insects.Add(obj);
                }
            }
        }
    }
}
