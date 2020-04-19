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

            SetDefaultHemisphere();

            SelectDataToLoad();
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

        private void SortPrice()
        {
            ThisMonthAnimals = ThisMonthAnimals.OrderByDescending(p => p.Price).ToList();
        }

        private void HideBook(RoutedEventArgs args)
        {
            var toggleSwitch = args.OriginalSource as ToggleSwitch;
            IsOwnedOn = toggleSwitch.IsOn;
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
            else
            {
                SelectDataToLoad();
            }
        }

        private void NowAvailable(RoutedEventArgs obj)
        {
            var toggleSwitch = obj.OriginalSource as ToggleSwitch;
            IsNowAvailableOn = toggleSwitch.IsOn;
            if (IsNowAvailableOn)
            {
                ThisMonthAnimals = ThisMonthAnimals.FindAll(delegate (NormalAnimal animals)
                {
                    var thisHour = DateTime.Now.Hour.ToString();
                    if (animals.Time.Contains(thisHour))
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

        private void GridViewItemClick(ItemClickEventArgs obj)
        {
            var animal = obj.ClickedItem as NormalAnimal;
            if (animal == null) return;
            UserControls.DetailControl detail = new UserControls.DetailControl(animal, _whichAnimal);
            detail.Show();
        }

        /// <summary>
        /// 加载北方鱼
        /// </summary>
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

        /// <summary>
        /// 加载北方昆虫
        /// </summary>
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

        /// <summary>
        /// 加载南方鱼
        /// </summary>
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

        /// <summary>
        /// 加载南方昆虫
        /// </summary>
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

        /// <summary>
        /// 选择加载何种数据
        /// </summary>
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

        /// <summary>
        /// 在拥有或者现在可获取开启情况下筛选
        /// </summary>
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
                    var thisHour = DateTime.Now.Hour.ToString();
                    if (animals.Time.Contains(thisHour))
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

        private void SetDefaultHemisphere()
        {
            var hemisphere = Helpers.SettingsHelper.GetLocalSetting(SettingsKey.DefaultHemisphereKey);
            if (hemisphere != null)
            {
                if (hemisphere == "North")
                {
                    IsNorthCheck = true;
                    IsSouthCheck = false;
                }
                else
                {
                    IsNorthCheck = false;
                    IsSouthCheck = true;
                }
            }
            else
            {
                IsNorthCheck = true;
                IsSouthCheck = false;
            }
        }
    }
}
