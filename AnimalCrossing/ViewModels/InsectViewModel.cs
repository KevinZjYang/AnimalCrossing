﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AnimalCrossing.Models;
using AnimalCrossing.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using System.Linq;

namespace AnimalCrossing.ViewModels
{
    public class InsectViewModel : ViewModelBase
    {
        private bool _beforeEditOwned;
        private bool _beforeEditMuseum;

        public List<PictorialBook> Pictorials { get; private set; }
        public List<Museum> Museums { get; private set; }
        public ObservableCollection<NormalAnimals> Insects { get; private set; } = new ObservableCollection<NormalAnimals>();

        private ICommand _southChecked;
        public ICommand SouthCheckedCommand
        {
            get
            {
                if (_southChecked == null)
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
                if (_beginningEdit == null)
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
                var normal = (obj.Row as FrameworkElement).DataContext as NormalAnimals;
                _beforeEditOwned = normal.Owned;
                _beforeEditMuseum = normal.MuseumHave;
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
                if (_cellEditEnded == null)
                {
                    _cellEditEnded = new RelayCommand<DataGridCellEditEndedEventArgs>(CellEditEnded);
                }
                return _cellEditEnded;
            }
        }

        private void CellEditEnded(DataGridCellEditEndedEventArgs obj)
        {
            var normal = (obj.Row as FrameworkElement).DataContext as NormalAnimals;
            SQLiteService.AddUserInsect(new UserInsect { Name = normal.Name, Owned = normal.Owned, MuseumHave = normal.MuseumHave });
            if (_beforeEditOwned == false && normal.Owned == true)
            {
                BookCount += 1;
            }
            if (_beforeEditOwned == true && normal.Owned == false)
            {
                BookCount -= 1;
            }

            if (_beforeEditMuseum == false && normal.MuseumHave == true)
            {
                MuseumCount += 1;
            }
            if (_beforeEditMuseum == true && normal.MuseumHave == false)
            {
                MuseumCount -= 1;
            }
        }

        private ICommand _sendEmail;
        public ICommand SendEmailCommand
        {
            get
            {
                if (_sendEmail == null)
                {
                    _sendEmail = new RelayCommand(SendEmail);
                }
                return _sendEmail;
            }
        }

        private async void SendEmail()
        {
            await Helpers.EmailHelper.UniversallyEmail("kevin.zj.yang@outlook.com", "动森图鉴问题反馈", "请输入要反馈的内容.如果关于数据错误的问题,请提交对应的证明材料.");
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

        private int _bookCount;
        public int BookCount
        {
            get { return _bookCount; }
            set { Set(ref _bookCount, value); }
        }

        private int _museumCount;
        public int MuseumCount
        {
            get { return _museumCount; }
            set { Set(ref _museumCount, value); }
        }

        public InsectViewModel()
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
            IsSouthCheck = false;
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
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
                                                                             orderby item.Number ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
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
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
                                                                             orderby item.Owned ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
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
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
                                                                             orderby item.MuseumHave ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
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
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
                                                                             orderby item.Price ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
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
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
                                                                             orderby item.Position ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
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
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
                                                                             orderby item.ShapeOrWeather ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
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
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
                                                                             orderby item.Time ascending
                                                                             select item);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    dg.ItemsSource = new ObservableCollection<NormalAnimals>(from item in Insects
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
            Insects.Clear();
            BookCount = 0;
            MuseumCount = 0;

            using (var con = SQLiteService.GetDbConnection())
            {
                var animals = con.Table<AnimalsInsect>().ToList();
                foreach (var item in animals)
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Insect>(item.Data);

                    List<UserInsect> userInsect;
                    using (var usercon = SQLiteService.GetUserDbConnection())
                    {
                        userInsect = usercon.Table<UserInsect>().Where(p => p.Name == item.Name).ToList();
                    }
                    if (userInsect.Count > 0)
                    {
                        var owned = userInsect[0].Owned;
                        var museumHave = userInsect[0].MuseumHave;

                        if (owned) BookCount += 1;
                        if (museumHave) MuseumCount += 1;

                        var normal = new NormalAnimals { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Weather, Time = obj.Time, AppearMonth = obj.Hemisphere.South.Month.AppearMonth, Owned = owned, MuseumHave = museumHave };
                        Insects.Add(normal);

                    }
                    else
                    {
                        var normal = new NormalAnimals { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Weather, Time = obj.Time, AppearMonth = obj.Hemisphere.South.Month.AppearMonth, Owned = false, MuseumHave = false };
                        Insects.Add(normal);

                    }

                }
            }

        }


        private void LoadNorthData()
        {
            Insects.Clear();
            BookCount = 0;
            MuseumCount = 0;

            using (var con = SQLiteService.GetDbConnection())
            {
                var animals = con.Table<AnimalsInsect>().ToList();
                foreach (var item in animals)
                {
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Insect>(item.Data);

                    List<UserInsect> userInsect;
                    using (var usercon = SQLiteService.GetUserDbConnection())
                    {
                        userInsect = usercon.Table<UserInsect>().Where(p => p.Name == item.Name).ToList();
                    }
                    if (userInsect.Count > 0)
                    {
                        var owned = userInsect[0].Owned;
                        var museumHave = userInsect[0].MuseumHave;

                        if (owned) BookCount += 1;
                        if (museumHave) MuseumCount += 1;

                        var normal = new NormalAnimals { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Weather, Time = obj.Time, AppearMonth = obj.Hemisphere.North.Month.AppearMonth, Owned = owned, MuseumHave = museumHave };
                        Insects.Add(normal);

                    }
                    else
                    {
                        var normal = new NormalAnimals { Name = obj.Name, Icon = $"ms-appx:///Icons/{obj.English}.jpg", Number = obj.Number, English = obj.English, Japanese = obj.Japanese, Price = Convert.ToInt32(obj.Price), Position = obj.Position, ShapeOrWeather = obj.Weather, Time = obj.Time, AppearMonth = obj.Hemisphere.North.Month.AppearMonth, Owned = false, MuseumHave = false };
                        Insects.Add(normal);

                    }

                }
            }

        }
    }
}
