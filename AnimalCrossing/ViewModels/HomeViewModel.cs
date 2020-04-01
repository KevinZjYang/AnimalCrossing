using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using AnimalCrossing.Models;
using AnimalCrossing.Services;
using System.Collections.Generic;

namespace AnimalCrossing.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private List<Insect> _insects;

        public ObservableCollection<Insect> ThisMonthInsects { get; private set; } = new ObservableCollection<Insect>();
        public ObservableCollection<Insect> NextMonthInsects { get; private set; } = new ObservableCollection<Insect>();

        public HomeViewModel()
        {
        }

        public void LoadDataAsync()
        {
            ThisMonthInsects.Clear();
            using (var con = SQLiteService.GetDbConnection())
            {
                var animals = con.Table<AnimalsInsect>().ToList();
                foreach (var item in animals)
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Insect>(item.Data);
                    var thisMonth = DateTime.Now.Month;
                    if (obj.Hemisphere.North.Month.AppearMonth[thisMonth - 1] == "1")
                    {
                        ThisMonthInsects.Add(obj);
                    }
                    //_insects.Add(obj);
                }
            }
        }
    }
}
