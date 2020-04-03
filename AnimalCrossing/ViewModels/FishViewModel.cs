using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AnimalCrossing.Models;
using AnimalCrossing.Services;
using GalaSoft.MvvmLight;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml;

namespace AnimalCrossing.ViewModels
{
    public class FishViewModel : ViewModelBase
    {
        public List<PictorialBook> Pictorials { get; private set; }
        public List<Museum> Museums { get; private set; }
        public ObservableCollection<NormalAnimals> Fishes { get; private set; } = new ObservableCollection<NormalAnimals>();

        private ICommand _southChecked;
        public ICommand SouthCheckedCommand
        {
            get
            {
                if(_southChecked== null)
                {
                    _southChecked = new RelayCommand<RoutedEventArgs>(SouthChecked);
                }
                return _southChecked;
            }
        }

        private void SouthChecked(RoutedEventArgs obj)
        {
            LoadSouthData();
        }

        private ICommand _northChecked;
        public ICommand NorthCheckedCommand
        {
            get
            {
                if (_northChecked == null)
                {
                    _northChecked = new RelayCommand<RoutedEventArgs>(NorthChecked);
                }
                return _northChecked;
            }
        }

        private void NorthChecked(RoutedEventArgs obj)
        {
            LoadNorthData();
        }

        private ICommand _beginningEdit;
        public ICommand BeginningEditCommand
        {
            get
            {
                if(_beginningEdit == null)
                {
                    _beginningEdit = new RelayCommand<DataGridBeginningEditEventArgs>(BeginningEdit);
                }
                return _beginningEdit;
            }
        }

        private void BeginningEdit(DataGridBeginningEditEventArgs obj)
        {
            if (obj.Column.Tag.ToString() == "Owned" || obj.Column.Tag.ToString() == "MuseumHave")
            {
                obj.Cancel = false;
            }
            else
            {
                obj.Cancel = true;
            }
        }

        private ICommand _cellEditEnded;
        public ICommand CellEditEndedCommand
        {
            get
            {
                if(_cellEditEnded == null)
                {
                    _cellEditEnded = new RelayCommand<DataGridCellEditEndedEventArgs>(CellEditEnded);
                }
                return _cellEditEnded;
            }
        }

        private void CellEditEnded(DataGridCellEditEndedEventArgs obj)
        {
            var normal = (obj.Row as FrameworkElement).DataContext as NormalAnimals;
            SQLiteService.AddUserFish(new UserFish { Name = normal.Name, Owned = normal.Owned, MuseumHave = normal.MuseumHave });
        }

        private bool _isNorthCheck;
        public bool IsNorthCheck
        {
            get { return _isNorthCheck; }
            set { Set(ref _isNorthCheck, value); }
        }

        private bool _isSouthCheck;
        public bool IsSouthCheck
        {
            get { return _isSouthCheck; }
            set { Set(ref _isSouthCheck, value); }
        }

        public FishViewModel()
        {
            Pictorials = new List<PictorialBook>
            {
                new PictorialBook{Owned = true,DisplayName = "是"},
                new PictorialBook{Owned =false,DisplayName = "否"}
            };
            Museums = new List<Museum>
            {
                new Museum{MuseumHave = true,DisplayName="是"},
                new Museum{MuseumHave=false,DisplayName="否"}
            };
        }

        public void LoadData()
        {

            IsSouthCheck =false;
            IsNorthCheck = true;
            LoadNorthData();
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

        private void LoadSouthData()
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
                        var normal = new NormalAnimals { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Shape, Time = obj.Time, AppearMonth = obj.Hemisphere.South.Month.AppearMonth, Owned = userFish[0].Owned, MuseumHave = userFish[0].MuseumHave };
                        Fishes.Add(normal);

                    }
                    else
                    {
                        var normal = new NormalAnimals { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Shape, Time = obj.Time, AppearMonth = obj.Hemisphere.South.Month.AppearMonth, Owned = false, MuseumHave = false };
                        Fishes.Add(normal);

                    }

                }
            }

        }


        private void LoadNorthData()
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
                        var normal = new NormalAnimals { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Shape, Time = obj.Time, AppearMonth = obj.Hemisphere.North.Month.AppearMonth, Owned = userFish[0].Owned, MuseumHave = userFish[0].MuseumHave };
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
    }
}
