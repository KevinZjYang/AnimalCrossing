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
using static AnimalCrossing.UserControls.DetailControl;

namespace AnimalCrossing.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private Animal _whichAnimal;
        private List<NormalAnimal> _thisMonthAnimals;

        public List<NormalAnimal> ThisMonthAnimals
        {
            get
            {
                if (_thisMonthAnimals == null)
                {
                    _thisMonthAnimals = new List<NormalAnimal>();
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
                ThisMonthAnimals = ThisMonthAnimals.FindAll(delegate (NormalAnimal animals)
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

        private ICommand _nowAvailable;

        public ICommand NowAvailableCommand
        {
            get
            {
                if (_nowAvailable == null)
                {
                    _nowAvailable = new RelayCommand<RoutedEventArgs>(NowAvailable);
                }
                return _nowAvailable;
            }
        }

        private void NowAvailable(RoutedEventArgs obj)
        {
            var toggleSwitch = obj.OriginalSource as ToggleSwitch;
            if (toggleSwitch.IsOn)
            {
                ThisMonthAnimals = ThisMonthAnimals.FindAll(delegate (NormalAnimal animals)
                {
                    if (Helpers.TimeHelper.JudgeIfHourInRange(animals.Time))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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

        private ICommand _itemClick;

        public ICommand ItemClickCommand
        {
            get
            {
                if (_itemClick == null)
                {
                    _itemClick = new RelayCommand<ItemClickEventArgs>(GridViewItemClick);
                }
                return _itemClick;
            }
        }

        private void GridViewItemClick(ItemClickEventArgs obj)
        {
            var animal = obj.ClickedItem as NormalAnimal;
            if (animal == null) return;
            UserControls.DetailControl detail = new UserControls.DetailControl(animal, _whichAnimal);
            detail.Show();
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

        private bool _isOwnedOn;

        public bool IsOwnedOn
        {
            get { return _isOwnedOn; }
            set { Set(ref _isOwnedOn, value); }
        }

        private bool _isNowAvailable;

        public bool IsNowAvailableOn
        {
            get { return _isNowAvailable; }
            set { Set(ref _isNowAvailable, value); }
        }

        #endregion

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
            List<NormalAnimal> animals = new List<NormalAnimal>();

            var normalAnimals = CommonDataService.GetThisMonthFishes(out int bookCount, out int museumCount, CommonDataService.Hemisphere.North);
            foreach (var item in normalAnimals)
            {
                animals.Add(item);
            }

            ThisMonthAnimals = animals;
            OwnedAndTimeSort();
            _whichAnimal = Animal.Fish;
        }

        private void LoadNorthInsectData()
        {
            List<NormalAnimal> animals = new List<NormalAnimal>();

            var normalAnimals = CommonDataService.GetThisMonthInsects(out int bookCount, out int museumCount, CommonDataService.Hemisphere.North);
            foreach (var item in normalAnimals)
            {
                animals.Add(item);
            }
            ThisMonthAnimals = animals;
            OwnedAndTimeSort();
            _whichAnimal = Animal.Insect;
        }

        private void LoadSouthFishData()
        {
            List<NormalAnimal> animals = new List<NormalAnimal>();

            var normalAnimals = CommonDataService.GetThisMonthFishes(out int bookCount, out int museumCount, CommonDataService.Hemisphere.South);
            foreach (var item in normalAnimals)
            {
                animals.Add(item);
            }
            ThisMonthAnimals = animals;
            OwnedAndTimeSort();
            _whichAnimal = Animal.Fish;
        }

        private void LoadSouthInsectData()
        {
            List<NormalAnimal> animals = new List<NormalAnimal>();
            var normalAnimals = CommonDataService.GetThisMonthInsects(out int bookCount, out int museumCount, CommonDataService.Hemisphere.South);
            foreach (var item in normalAnimals)
            {
                animals.Add(item);
            }
            ThisMonthAnimals = animals;
            OwnedAndTimeSort();
            _whichAnimal = Animal.Insect;
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

        private void OwnedAndTimeSort()
        {
            if (IsOwnedOn)
            {
                ThisMonthAnimals = ThisMonthAnimals.FindAll(delegate (NormalAnimal animals)
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
            if (IsNowAvailableOn)
            {
                ThisMonthAnimals = ThisMonthAnimals.FindAll(delegate (NormalAnimal animals)
                {
                    if (Helpers.TimeHelper.JudgeIfHourInRange(animals.Time))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
        }
    }
}
