using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using AnimalCrossing.Models;
using AnimalCrossing.Services;
using System.Collections.Generic;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace AnimalCrossing.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        //private List<Insect> _insects;

        //public ObservableCollection<Insect> ThisMonthInsects { get; private set; } = new ObservableCollection<Insect>();

        private List<NormalAnimals> _thisMonthAnimals;

        public List<NormalAnimals> ThisMonthAnimals
        {
            get
            {
                if (_thisMonthAnimals == null)
                {
                    _thisMonthAnimals = new List<NormalAnimals>();
                }
                return _thisMonthAnimals;
            }
            set { Set(ref _thisMonthAnimals, value); }
        }

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
            if (IsFishCheck)
            {
                LoadSouthFishData();
            }
            else
            {
                LoadSouthInsectData();
            }
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
            if (IsFishCheck)
            {
                LoadNorthFishData();
            }
            else
            {
                LoadNorthInsectData();
            }
        }

        private ICommand _sortPrice;

        public ICommand SortPriceCommand
        {
            get
            {
                if (_sortPrice == null)
                {
                    _sortPrice = new RelayCommand(SortPrice);
                }
                return _sortPrice;
            }
        }

        private void SortPrice()
        {
            ThisMonthAnimals = ThisMonthAnimals.OrderByDescending(p => p.Price).ToList();
        }

        private ICommand _hideBook;

        public ICommand HideBookCommand
        {
            get
            {
                if (_hideBook == null)
                {
                    _hideBook = new RelayCommand<RoutedEventArgs>(HideBook);
                }
                return _hideBook;
            }
        }

        private void HideBook(RoutedEventArgs args)
        {
            var toggleSwitch = args.OriginalSource as ToggleSwitch;
            if (toggleSwitch.IsOn)
            {
                ThisMonthAnimals = ThisMonthAnimals.FindAll(delegate (NormalAnimals animals)
                {
                    if (animals.Owned)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                });
            }
            else
            {
                SelectDataToLoad();
            }
        }

        private ICommand _fishChecked;

        public ICommand FishCheckedCommand
        {
            get
            {
                if (_fishChecked == null)
                {
                    _fishChecked = new RelayCommand<RoutedEventArgs>(FishChecked);
                }
                return _fishChecked;
            }
        }

        private void FishChecked(RoutedEventArgs obj)
        {
            if (IsNorthCheck)
            {
                LoadNorthFishData();
            }
            else
            {
                LoadSouthFishData();
            }
        }

        private ICommand _insectChecked;

        public ICommand InsectCheckedCommand
        {
            get
            {
                if (_insectChecked == null)
                {
                    _insectChecked = new RelayCommand<RoutedEventArgs>(InsectChecked);
                }
                return _insectChecked;
            }
        }

        private void InsectChecked(RoutedEventArgs obj)
        {
            if (IsNorthCheck)
            {
                LoadNorthInsectData();
            }
            else
            {
                LoadSouthInsectData();
            }
        }

        #region
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

        private bool _isFishCheck;

        public bool IsFishCheck
        {
            get { return _isFishCheck; }
            set { Set(ref _isFishCheck, value); }
        }

        private bool _isInsectCheck;

        public bool IsInsectCheck
        {
            get { return _isInsectCheck; }
            set { Set(ref _isInsectCheck, value); }
        }

        #endregion

        public ObservableCollection<Insect> NextMonthInsects { get; private set; } = new ObservableCollection<Insect>();

        public HomeViewModel()
        {
        }

        public void LoadDataAsync()
        {
            IsFishCheck = true;
            IsInsectCheck = false;

            IsNorthCheck = true;
            IsSouthCheck = false;

            SelectDataToLoad();
        }

        private void LoadNorthFishData()
        {
            List<NormalAnimals> animals = new List<NormalAnimals>();

            var normalAnimals = CommonDataService.GetThisMonthFishes(out int bookCount, out int museumCount, CommonDataService.Hemisphere.North);
            foreach (var item in normalAnimals)
            {
                animals.Add(item);
            }

            ThisMonthAnimals = animals;
        }

        private void LoadNorthInsectData()
        {
            List<NormalAnimals> animals = new List<NormalAnimals>();

            var normalAnimals = CommonDataService.GetThisMonthInsects(out int bookCount, out int museumCount, CommonDataService.Hemisphere.North);
            foreach (var item in normalAnimals)
            {
                animals.Add(item);
            }
            ThisMonthAnimals = animals;
        }

        private void LoadSouthFishData()
        {
            List<NormalAnimals> animals = new List<NormalAnimals>();

            var normalAnimals = CommonDataService.GetThisMonthFishes(out int bookCount, out int museumCount, CommonDataService.Hemisphere.South);
            foreach (var item in normalAnimals)
            {
                animals.Add(item);
            }
            ThisMonthAnimals = animals;
        }

        private void LoadSouthInsectData()
        {
            List<NormalAnimals> animals = new List<NormalAnimals>();
            var normalAnimals = CommonDataService.GetThisMonthInsects(out int bookCount, out int museumCount, CommonDataService.Hemisphere.South);
            foreach (var item in normalAnimals)
            {
                animals.Add(item);
            }
            ThisMonthAnimals = animals;
        }

        private void SelectDataToLoad()
        {
            if (IsFishCheck)
            {
                if (IsNorthCheck)
                {
                    LoadNorthFishData();
                }
                else
                {
                    LoadSouthFishData();
                }
            }
            else
            {
                if (IsNorthCheck)
                {
                    LoadNorthInsectData();
                }
                else
                {
                    LoadSouthInsectData();
                }
            }
        }
    }
}
