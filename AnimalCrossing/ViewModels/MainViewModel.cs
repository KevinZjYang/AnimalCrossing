using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using AnimalCrossing.Models;
using AnimalCrossing.Views;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace AnimalCrossing.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Frame _rootFrame;
        public ObservableCollection<NavItem> NavItems { get; private set; } = new ObservableCollection<NavItem>();

        private ICommand _itemClick;

        public ICommand ItemClickCommand
        {
            get
            {
                if (_itemClick == null)
                {
                    _itemClick = new RelayCommand<ItemClickEventArgs>(ItemClick);
                }
                return _itemClick;
            }
        }

        private void ItemClick(ItemClickEventArgs obj)
        {
            var item = obj.ClickedItem as NavItem;
            if (item == null) return;
            Nav(item.PageFullName);
        }

        private ICommand _settingsTap;

        public ICommand SettingsTapCommand
        {
            get
            {
                if (_settingsTap == null)
                {
                    _settingsTap = new RelayCommand(SettingTapped);
                }
                return _settingsTap;
            }
        }

        private void SettingTapped()
        {
            Nav(typeof(SettingsPage).FullName);
        }

        public MainViewModel()
        {
            NavItems.Add(new NavItem { Name = "主页", PageFullName = typeof(HomePage).FullName });
            NavItems.Add(new NavItem { Name = "大头菜", PageFullName = typeof(PricePage).FullName });
            NavItems.Add(new NavItem { Name = "鱼类", PageFullName = typeof(FishPage).FullName });
            NavItems.Add(new NavItem { Name = "昆虫", PageFullName = typeof(InsectPage).FullName });
            NavItems.Add(new NavItem { Name = "小动物", PageFullName = typeof(LittleAnimalPage).FullName });
            NavItems.Add(new NavItem { Name = "植物", PageFullName = typeof(PlantPage).FullName });
            NavItems.Add(new NavItem { Name = "唱片", PageFullName = typeof(AlbumPage).FullName });
        }

        public void Initialize(Frame frame)
        {
            _rootFrame = frame;
            _rootFrame.NavigationFailed += _rootFrame_NavigationFailed;
            Nav(typeof(HomePage).FullName);
        }

        private void _rootFrame_NavigationFailed(object sender, Windows.UI.Xaml.Navigation.NavigationFailedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 导航到页面
        /// </summary>
        /// <param name="pageFullName">
        /// 页面的全称
        /// </param>
        private void Nav(string pageFullName)
        {
            var pageType = Type.GetType(pageFullName);
            _rootFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }
}
