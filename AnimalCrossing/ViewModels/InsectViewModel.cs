using System;
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

            #endregion Sort Number

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

            #endregion Sort Owned

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

            #endregion Sort MuseumHave

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

            #endregion Sort Price

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

            #endregion Sort Position

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

            #endregion Sort ShapOrWeather

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

            #endregion Sort Time

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

            var normalAnimals = CommonDataService.GetAllInsects(out int bookCount, out int museumCount, CommonDataService.Hemisphere.North);
            BookCount = bookCount;
            MuseumCount = museumCount;
            foreach (var item in normalAnimals)
            {
                Insects.Add(item);
            }
        }

        private void LoadNorthData()
        {
            Insects.Clear();
            BookCount = 0;
            MuseumCount = 0;

            var normalAnimals = CommonDataService.GetAllInsects(out int bookCount, out int museumCount, CommonDataService.Hemisphere.North);
            BookCount = bookCount;
            MuseumCount = museumCount;
            foreach (var item in normalAnimals)
            {
                Insects.Add(item);
            }
        }
    }
}
