using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AnimalCrossing.Models;
using AnimalCrossing.Services;
using GalaSoft.MvvmLight;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace AnimalCrossing.ViewModels
{
    public class FishViewModel : ViewModelBase
    {
        public ObservableCollection<NormalAnimals> Fishes { get; private set; } = new ObservableCollection<NormalAnimals>();

        public FishViewModel()
        {
        }

        public void LoadData()
        {
            Fishes.Clear();
            using (var con = SQLiteService.GetDbConnection())
            {
                var animals = con.Table<AnimalsFish>().ToList();
                foreach (var item in animals)
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Fish>(item.Data);

                    List<UserFish> userFish;
                    using (var usercon = SQLiteService.GetUserDbConnection())
                    {
                       userFish = usercon.Table<UserFish>().Where(p => p.Name == item.Name).ToList();
                    }
                    if (userFish.Count > 0)
                    {
                        var normal = new NormalAnimals { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Shape, Time = obj.Time, AppearMonth = obj.Hemisphere.North.Month.AppearMonth,Owned=userFish[0].Owned,MuseumHave=userFish[0].MuseumHave};
                        Fishes.Add(normal);

                    }
                    else
                    {
                        var normal = new NormalAnimals { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Shape, Time = obj.Time, AppearMonth = obj.Hemisphere.North.Month.AppearMonth, Owned = false, MuseumHave = false };
                        Fishes.Add(normal);

                    }
                   
                }
            }
         
        }

        public void OnSorting(object sender, DataGridColumnEventArgs e)
        {
            var dg = sender as DataGrid;

            #region Sort Number
            if (e.Column.Tag.ToString() == "Number")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Number ascending
                                                                     select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Number descending
                                                                     select item);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
                  
            }
            #endregion

            #region Sort Name
            if (e.Column.Tag.ToString() == "Name")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Name ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Name descending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }

            }
            #endregion

            #region Sort Owned
            if (e.Column.Tag.ToString() == "Owned")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Owned ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Owned descending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
            }
            #endregion

            #region Sort MuseumHave
            if (e.Column.Tag.ToString() == "MuseumHave")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.MuseumHave ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.MuseumHave descending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }

            }
            #endregion

            #region Sort Price
            if (e.Column.Tag.ToString() == "Price")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Price ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Price descending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }

            }
            #endregion

            #region Sort Position
            if (e.Column.Tag.ToString() == "Position")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Position ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Position descending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }

            }
            #endregion

            #region Sort ShapOrWeather
            if (e.Column.Tag.ToString() == "ShapeOrWeather")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.ShapeOrWeather ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.ShapeOrWeather descending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }

            }
            #endregion

            #region Sort Time
            if (e.Column.Tag.ToString() == "Time")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Time ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Fishes
                                                                             orderby item.Time descending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }

            }
            #endregion

            foreach (var dgColumn in dg.Columns)
            {
                if (dgColumn.Tag.ToString() != e.Column.Tag.ToString())
                {
                    dgColumn.SortDirection = null;
                }
            }

        }


    }
}
